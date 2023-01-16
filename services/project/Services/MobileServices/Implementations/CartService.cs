using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using FirebaseAdmin.Auth.Multitenancy;
using Infrastructure.Abstractions;
using Models.Db.DbCart;
using Models.DTOs.Carts;
using Models.Misc;
using Services.ExternalServices;
using Services.MobileServices.Abstractions;

namespace Services.MobileServices.Implementations
{
    public class CartService : ICartService
    {
        private readonly ICartItemRepository _cartItemRepository;

        private readonly IMapper _mapper;

        public CartService(ICartItemRepository cartItemRepository, IMapper mapper)
        {
            _cartItemRepository = cartItemRepository;
            _mapper = mapper;
        }

        public async Task Upload(UploadCartItemsDto uploadCartItemsDto)
        {
            await ClearByClient(uploadCartItemsDto.ClientAccountId);

            foreach (var addCartItemDto in uploadCartItemsDto.CartItems)
            {
                // Cart item doen't exist, create one
                var cartItem = _mapper.Map<CartItem>(addCartItemDto);
                await _cartItemRepository.Add(cartItem);
            }
        }

        public async Task AddItem(AddCartItemDto addCartItemDto)
        {
            var cartItem = await _cartItemRepository.GetOne(i => i.ClientAccountId == addCartItemDto.ClientAccountId && i.MenuItemId == addCartItemDto.MenuItemId);

            if (cartItem == null)
            {
                // Cart item doen't exist, create one
                cartItem = _mapper.Map<CartItem>(addCartItemDto);
                await _cartItemRepository.Add(cartItem);
            }
            else
            {
                cartItem.Amount += addCartItemDto.Amount;
                await _cartItemRepository.Update(cartItem);
            }
        }

        public async Task RemoveItem(RemoveCartItemDto removeCartItemDto)
        {
            var cartItem = await _cartItemRepository.GetOne(i =>
                i.ClientAccountId == removeCartItemDto.ClientAccountId &&
                i.MenuItemId == removeCartItemDto.MenuItemId
            );

            // Cart item doen't exist, this is an exception
            cartItem.EnsureNotNullHandled("CartItem is not in User's cart");

            if (removeCartItemDto.Amount > cartItem.Amount)
            {
                await TelegramAPI.Send("Mobile/Cart/RemoveItem:\nAttempt to remove more elements than exists");
                throw new AkianaException("Вы пытаетесь удалить из корзины больше позиций, чем есть!");
            }

            cartItem.Amount -= removeCartItemDto.Amount;

            if (cartItem.Amount == 0)
            {
                await _cartItemRepository.Remove(cartItem);
            }
            else
            {
                await _cartItemRepository.Update(cartItem);
            }
        }

        public async Task ClearByClient(long id)
        {
            var cartItems = await _cartItemRepository.GetMany(i => i.ClientAccountId == id);

            foreach (var cartItem in cartItems)
            {
                cartItem.Amount = 0;
            }
            
            await _cartItemRepository.RemoveMany(cartItems);
        }

        public async Task<ICollection<MobileCartItemDto>> GetByClient(long id)
        {
            var cartItems = await _cartItemRepository.GetManyNonTracking(i => i.ClientAccountId == id, i => i.MenuItem);

            var cartItemDtos = _mapper.Map<ICollection<MobileCartItemDto>>(cartItems);

            return cartItemDtos;
        }
    }
}