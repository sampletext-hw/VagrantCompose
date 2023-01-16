using System;
using System.Linq;
using AutoMapper;
using Models.Db;
using Models.Db.Account;
using Models.Db.Common;
using Models.Db.CompanyInfo;
using Models.Db.DbCart;
using Models.Db.DbOrder;
using Models.Db.DbRestaurant;
using Models.Db.LatLngs;
using Models.Db.Menu;
using Models.Db.MobilePushes;
using Models.Db.Relations;
using Models.Db.RestaurantStop;
using Models.Db.Schedule;
using Models.DTOs.Banners;
using Models.DTOs.Carts;
using Models.DTOs.Categories;
using Models.DTOs.Cities;
using Models.DTOs.ClientAccounts;
using Models.DTOs.CompanyInfos;
using Models.DTOs.CompanyInfos.AboutData;
using Models.DTOs.CompanyInfos.ApplicationStartupImageData;
using Models.DTOs.CompanyInfos.ApplicationTermination;
using Models.DTOs.CompanyInfos.Common;
using Models.DTOs.CompanyInfos.DeliveryTermsData;
using Models.DTOs.CompanyInfos.InstagramUrlData;
using Models.DTOs.CompanyInfos.VacanciesData;
using Models.DTOs.CompanyInfos.VkUrlData;
using Models.DTOs.DeliveryAddresses;
using Models.DTOs.Favorite;
using Models.DTOs.General;
using Models.DTOs.LatLngDtos;
using Models.DTOs.MeasureDto;
using Models.DTOs.MenuItems;
using Models.DTOs.MenuItemToMenuProductDto;
using Models.DTOs.MenuItemToPriceGroupDto;
using Models.DTOs.MenuProducts;
using Models.DTOs.Misc;
using Models.DTOs.MobilePushes;
using Models.DTOs.Orders;
using Models.DTOs.PriceGroups;
using Models.DTOs.Restaurants;
using Models.DTOs.RestaurantStops;
using Models.DTOs.WorkerAccountDtos;
using Models.DTOs.WorkerRoles;

namespace Services.AutoMapperProfiles
{
    // --------------------------------------------------------- //
    // EVEN IF YOUR IDE SAYS THIS CODE IS UNUSED, DONT DELETE IT //
    // --------------------------------------------------------- //

    public class AkianaAutomapperProfile : Profile
    {
        public AkianaAutomapperProfile()
        {
            // ReverseMap() нужен для обратной конвертации любого мапа

            // CreateMap<string, TimeSpan>().ConvertUsing(s => TimeSpan.ParseExact(s, "hh\\:mm", null));
            // CreateMap<TimeSpan, string>().ConvertUsing(time => $"{time.Hours:00}:{time.Minutes:00}");

            // -----------
            // This doesn't work for some reason, need to specify every derived type
            // CreateMap<LatLngDto, LatLng>().ReverseMap();
            // -----------

            CreateMap<RestaurantLatLng, RestaurantLatLngDto>().ReverseMap();
            CreateMap<DeliveryZoneLatLng, DeliveryZoneLatLngDto>().ReverseMap();
            CreateMap<DeliveryAddressLatLng, DeliveryAddressLatLngDto>().ReverseMap();
            CreateMap<CityLatLng, CityLatLngDto>().ReverseMap();

            CreateMap<WorkerAccountToRestaurant, IdDto>()
                .ForMember(dto => dto.Id, cfg => cfg.MapFrom(r => r.RestaurantId))
                .ReverseMap()
                .ForMember(r => r.RestaurantId, cfg => cfg.MapFrom(dto => dto.Id));

            CreateMap<WorkerAccountToRole, IdDto>()
                .ForMember(dto => dto.Id, cfg => cfg.MapFrom(w => w.WorkerRoleId))
                .ReverseMap()
                .ForMember(w => w.WorkerRoleId, cfg => cfg.MapFrom(dto => dto.Id));
            
            CreateMap<WorkerAccount, CreateWorkerAccountDto>()
                .ForMember(dto => dto.Restaurants, cfg => cfg.MapFrom(w => w.RestaurantsRelation))
                .ForMember(dto => dto.WorkerRoles, cfg => cfg.MapFrom(w => w.WorkerRolesRelation))
                .ReverseMap()
                .ForMember(dst => dst.IsTechnical, cfg => cfg.Ignore())
                .ForMember(w => w.Restaurants, cfg => cfg.Ignore())
                .ForMember(w => w.WorkerRoles, cfg => cfg.Ignore())
                .ForMember(w => w.RestaurantsRelation, cfg => cfg.MapFrom(dto => dto.Restaurants))
                .ForMember(w => w.WorkerRolesRelation, cfg => cfg.MapFrom(dto => dto.WorkerRoles));
            
            CreateMap<WorkerAccount, UpdateWorkerAccountDto>()
                .ForMember(dto => dto.Restaurants, cfg => cfg.MapFrom(w => w.RestaurantsRelation))
                .ForMember(dto => dto.WorkerRoles, cfg => cfg.MapFrom(w => w.WorkerRolesRelation))
                .ReverseMap()
                .ForMember(dst => dst.IsTechnical, cfg => cfg.Ignore())
                .ForMember(w => w.Restaurants, cfg => cfg.Ignore())
                .ForMember(w => w.WorkerRoles, cfg => cfg.Ignore())
                .ForMember(w => w.RestaurantsRelation, cfg => cfg.MapFrom(dto => dto.Restaurants))
                .ForMember(w => w.WorkerRolesRelation, cfg => cfg.MapFrom(dto => dto.WorkerRoles));
            
            CreateMap<WorkerAccount, WorkerAccountWithIdDto>()
                .ForMember(dto => dto.Restaurants, cfg => cfg.MapFrom(w => w.RestaurantsRelation))
                .ForMember(dto => dto.WorkerRoles, cfg => cfg.MapFrom(w => w.WorkerRolesRelation))
                .ReverseMap()
                .ForMember(dst => dst.IsTechnical, cfg => cfg.Ignore())
                .ForMember(w => w.Restaurants, cfg => cfg.Ignore())
                .ForMember(w => w.WorkerRoles, cfg => cfg.Ignore())
                .ForMember(w => w.RestaurantsRelation, cfg => cfg.MapFrom(dto => dto.Restaurants))
                .ForMember(w => w.WorkerRolesRelation, cfg => cfg.MapFrom(dto => dto.WorkerRoles));

            CreateMap<WorkerRole, WorkerRoleDto>().ReverseMap();

            CreateMap<CategoryMobileDto, Category>().ReverseMap();
            CreateMap<CategoryWithIdDto, Category>().ReverseMap();
            CreateMap<CreateCategoryDto, Category>().ReverseMap();
            CreateMap<UpdateCategoryDto, Category>().ReverseMap();

            CreateMap<PriceGroupWithIdDto, PriceGroup>().ReverseMap();
            CreateMap<CreatePriceGroupDto, PriceGroup>().ReverseMap();
            CreateMap<UpdatePriceGroupDto, PriceGroup>().ReverseMap();

            CreateMap<CityMobileDto, City>().ReverseMap();

            CreateMap<CityWithIdDto, City>().ReverseMap();
            CreateMap<CreateCityDto, City>().ReverseMap();
            CreateMap<UpdateCityDto, City>().ReverseMap();

            CreateMap<RestaurantMobileDto, Restaurant>().ReverseMap()
                .ForMember(dto => dto.IsDeliveryWorking, cfg => cfg.MapFrom(r =>
                    r.DeliveryStops.LastOrDefault() == null || r.DeliveryStops.LastOrDefault().EndDate != null
                ))
                .ForMember(dto => dto.LastDeliveryStopReason, cfg => cfg.MapFrom(r => r.DeliveryStops.LastOrDefault().Reason))
                .ForMember(dto => dto.IsPickupWorking, cfg => cfg.MapFrom(r =>
                    r.PickupStops.LastOrDefault() == null || r.PickupStops.LastOrDefault().EndDate != null
                ))
                .ForMember(dto => dto.LastPickupStopReason, cfg => cfg.MapFrom(r => r.PickupStops.LastOrDefault().Reason))
                .ForMember(dto => dto.IsWorking, cfg => cfg.MapFrom(r =>
                    (r.DeliveryStops.LastOrDefault() == null || r.DeliveryStops.LastOrDefault().EndDate != null) ||
                    (r.PickupStops.LastOrDefault() == null || r.PickupStops.LastOrDefault().EndDate != null)
                ))
                .ForMember(dst => dst.SupportedPaymentTypes, cfg => cfg.MapFrom(src => src.SupportedPaymentTypes.ToBits()));

            CreateMap<RestaurantWithIdDto, Restaurant>()
                .ReverseMap()
                .ForMember(dto => dto.IsDeliveryWorking, cfg => cfg.MapFrom(r =>
                    r.DeliveryStops.LastOrDefault() == null || r.DeliveryStops.LastOrDefault().EndDate != null
                ))
                .ForMember(dto => dto.IsPickupWorking, cfg => cfg.MapFrom(r =>
                    r.PickupStops.LastOrDefault() == null || r.PickupStops.LastOrDefault().EndDate != null
                ))
                .ForMember(dto => dto.IsWorking, cfg => cfg.MapFrom(r =>
                    (r.DeliveryStops.LastOrDefault() == null || r.DeliveryStops.LastOrDefault().EndDate != null) ||
                    (r.PickupStops.LastOrDefault() == null || r.PickupStops.LastOrDefault().EndDate != null)
                ))
                .ForMember(dst => dst.SupportedPaymentTypes, cfg => cfg.MapFrom(src => src.SupportedPaymentTypes.ToBits()));

            CreateMap<CreateRestaurantDto, Restaurant>()
                .ForMember(dst => dst.SupportedPaymentTypes, cfg => cfg.MapFrom(src => src.SupportedPaymentTypes.FromBits()))
                .ReverseMap();
            CreateMap<UpdateRestaurantDto, Restaurant>()
                .ForMember(dst => dst.SupportedPaymentTypes, cfg => cfg.MapFrom(src => src.SupportedPaymentTypes.FromBits()))
                .ReverseMap();

            CreateMap<MenuItemToPriceGroup, MenuItemToPriceGroupRelationDto>().ReverseMap();
            CreateMap<MenuItemToMenuProduct, MenuItemToMenuProductRelationDto>().ReverseMap();

            CreateMap<MenuItemWithIdDto, MenuItem>()
                // Map price group with price inside
                .ForMember(item => item.PriceGroupsRelation, cfg => cfg.MapFrom(dto => dto.Prices))
                .ForMember(item => item.MenuProductsRelation, cfg => cfg.MapFrom(dto => dto.Products))
                .ReverseMap()
                // Map price group
                .ForMember(dto => dto.Prices, cfg => cfg.MapFrom(item => item.PriceGroupsRelation))
                .ForMember(dto => dto.Products, cfg => cfg.MapFrom(item => item.MenuProductsRelation));

            CreateMap<MenuItem, MenuItemMobileDto>()
                .ForMember(dto => dto.Price, cfg => cfg.MapFrom(item => item.PriceGroupsRelation.FirstOrDefault().Price))
                .ForMember(dto => dto.Products, cfg => cfg.MapFrom(item => item.MenuProductsRelation))
                .ReverseMap();

            CreateMap<CreateMenuItemDto, MenuItem>()
                // Map price group with price inside
                .ForMember(item => item.PriceGroupsRelation, cfg => cfg.MapFrom(dto => dto.Prices))
                .ForMember(item => item.MenuProductsRelation, cfg => cfg.MapFrom(dto => dto.Products))
                .ReverseMap()
                // Map price group
                .ForMember(dto => dto.Prices, cfg => cfg.MapFrom(item => item.PriceGroupsRelation))
                .ForMember(dto => dto.Products, cfg => cfg.MapFrom(item => item.MenuProductsRelation));

            CreateMap<UpdateMenuItemDto, MenuItem>()
                .ForMember(item => item.PriceGroupsRelation, cfg => cfg.MapFrom(dto => dto.Prices))
                .ForMember(item => item.MenuProductsRelation, cfg => cfg.MapFrom(dto => dto.Products))
                .ReverseMap()
                .ForMember(dto => dto.Prices, cfg => cfg.MapFrom(item => item.PriceGroupsRelation))
                .ForMember(dto => dto.Products, cfg => cfg.MapFrom(item => item.MenuProductsRelation));

            CreateMap<MenuProduct, MenuProductMobileDto>().ReverseMap();

            CreateMap<MenuProductWithIdDto, MenuProduct>().ReverseMap();
            CreateMap<CreateMenuProductDto, MenuProduct>().ReverseMap();
            CreateMap<UpdateMenuProductDto, MenuProduct>().ReverseMap();

            CreateMap<BannerToCity, IdDto>()
                .ForMember(dto => dto.Id, cfg => cfg.MapFrom(r => r.CityId))
                .ReverseMap()
                .ForMember(r => r.CityId, cfg => cfg.MapFrom(dto => dto.Id));

            CreateMap<BannerWithIdDto, Banner>()
                .ForMember(b => b.Cities, cfg => cfg.Ignore())
                .ForMember(b => b.CitiesRelation, cfg => cfg.MapFrom(dto => dto.Cities))
                .ReverseMap()
                .ForMember(dto => dto.Cities, cfg => cfg.MapFrom(b => b.CitiesRelation));
            CreateMap<CreateBannerDto, Banner>()
                .ForMember(b => b.Cities, cfg => cfg.Ignore())
                .ForMember(b => b.CitiesRelation, cfg => cfg.MapFrom(dto => dto.Cities))
                .ReverseMap()
                .ForMember(dto => dto.Cities, cfg => cfg.MapFrom(b => b.CitiesRelation));
            CreateMap<UpdateBannerDto, Banner>()
                .ForMember(b => b.Cities, cfg => cfg.Ignore())
                .ForMember(b => b.CitiesRelation, cfg => cfg.MapFrom(dto => dto.Cities))
                .ReverseMap()
                .ForMember(dto => dto.Cities, cfg => cfg.MapFrom(b => b.CitiesRelation));
            CreateMap<BannerMobileDto, Banner>().ReverseMap();

            CreateMap<DeliveryOpenCloseTime, OpenCloseTimeDto>().ReverseMap();
            CreateMap<PickupOpenCloseTime, OpenCloseTimeDto>().ReverseMap();

            CreateMap<CPFCDto, MenuCPFC>().ReverseMap();

            CreateMap<MenuItemMeasureDto, MenuItemMeasure>().ReverseMap();

            CreateMap<AboutDataDto, AboutData>().ReverseMap();
            CreateMap<DeliveryTermsDataDto, DeliveryTermsData>().ReverseMap();
            CreateMap<VacanciesDataDto, VacanciesData>().ReverseMap();
            CreateMap<ApplicationStartupImageDataDto, ApplicationStartupImageData>().ReverseMap();
            CreateMap<ApplicationTerminationDto, ApplicationTerminationData>().ReverseMap();
            CreateMap<VkUrlDataDto, VkUrlData>().ReverseMap();
            CreateMap<InstagramUrlDataDto, InstagramUrlData>().ReverseMap();

            CreateMap<MobileAboutDataDto, AboutData>().ReverseMap();
            CreateMap<MobileApplicationStartupImageDataDto, ApplicationStartupImageData>().ReverseMap();
            CreateMap<MobileDeliveryTermsDataDto, DeliveryTermsData>().ReverseMap();
            CreateMap<MobileVacanciesDataDto, VacanciesData>().ReverseMap();
            CreateMap<MobileApplicationTerminationDto, ApplicationTerminationData>().ReverseMap();
            CreateMap<MobileVkUrlDataDto, VkUrlData>().ReverseMap();
            CreateMap<MobileInstagramUrlDataDto, InstagramUrlData>().ReverseMap();

            CreateMap<UpdateAboutDataDto, AboutData>().ReverseMap();
            CreateMap<UpdateDeliveryTermsDataDto, DeliveryTermsData>().ReverseMap();
            CreateMap<UpdateVacanciesDataDto, VacanciesData>().ReverseMap();
            CreateMap<UpdateApplicationStartupImageDataDto, ApplicationStartupImageData>().ReverseMap();
            CreateMap<UpdateApplicationTerminationDto, ApplicationTerminationData>().ReverseMap();
            CreateMap<UpdateVkUrlDataDto, VkUrlData>().ReverseMap();
            CreateMap<UpdateInstagramUrlDataDto, InstagramUrlData>().ReverseMap();

            CreateMap<MobilePushToCity, IdDto>()
                .ForMember(dto => dto.Id, cfg => cfg.MapFrom(n => n.CityId))
                .ReverseMap()
                .ForMember(n => n.CityId, cfg => cfg.MapFrom(dto => dto.Id));

            CreateMap<MobilePushToPriceGroup, IdDto>()
                .ForMember(dto => dto.Id, cfg => cfg.MapFrom(n => n.PriceGroupId))
                .ReverseMap()
                .ForMember(n => n.PriceGroupId, cfg => cfg.MapFrom(dto => dto.Id));

            CreateMap<MobilePushByCity, MobilePushWithIdDto>()
                .ForMember(dto => dto.Targets, cfg => cfg.MapFrom(n => n.CitiesRelation))
                .ReverseMap()
                .ForMember(n => n.CitiesRelation, cfg => cfg.MapFrom(dto => dto.Targets));

            CreateMap<MobilePushByPriceGroup, MobilePushWithIdDto>()
                .ForMember(dto => dto.Targets, cfg => cfg.MapFrom(n => n.PriceGroupsRelation))
                .ReverseMap()
                .ForMember(n => n.PriceGroupsRelation, cfg => cfg.MapFrom(dto => dto.Targets));

            CreateMap<MobilePushByCity, CreateMobilePushDto>()
                .ForMember(dto => dto.Targets, cfg => cfg.MapFrom(n => n.CitiesRelation))
                .ReverseMap()
                .ForMember(n => n.CitiesRelation, cfg => cfg.MapFrom(dto => dto.Targets));

            CreateMap<MobilePushByPriceGroup, CreateMobilePushDto>()
                .ForMember(dto => dto.Targets, cfg => cfg.MapFrom(n => n.PriceGroupsRelation))
                .ReverseMap()
                .ForMember(n => n.PriceGroupsRelation, cfg => cfg.MapFrom(dto => dto.Targets));

            CreateMap<ClientAccount, ClientAccountDto>()
                .ReverseMap();

            CreateMap<ClientAccount, MobileClientAccountDto>().ReverseMap();
            CreateMap<ClientLoginRequest, MobileClientLoginRequestDto>().ReverseMap();

            CreateMap<DeliveryAddress, DeliveryAddressDto>().ReverseMap();
            CreateMap<DeliveryAddress, DeliveryAddressMobileDto>().ReverseMap();
            CreateMap<DeliveryAddress, AddDeliveryAddressDto>().ReverseMap();
            CreateMap<DeliveryAddress, UpdateDeliveryAddressDto>().ReverseMap();

            CreateMap<CartItem, MobileCartItemDto>().ReverseMap();
            CreateMap<CartItem, AddCartItemDto>().ReverseMap();

            CreateMap<FavoriteItem, MobileFavoriteItemDto>().ReverseMap();
            CreateMap<FavoriteItem, AddFavoriteItemDto>().ReverseMap();

            CreateMap<Order, MobileCreateOrderFromCartDto>().ReverseMap()
                .ForMember(dst => dst.PaymentType, cfg => cfg.MapFrom(src => src.PaymentType.ToDb()));
            CreateMap<Order, MobileCreateOrderFromCartV2Dto>().ReverseMap();
            CreateMap<Order, OrderDto>()
                .ForMember(dto => dto.TotalCost, cfg => cfg.MapFrom(o => o.OrderItems.Sum(i => i.PurchasePrice)))
                .ReverseMap();
            CreateMap<Order, OrderMobileDto>()
                .ForMember(dto => dto.TotalCost, cfg => cfg.MapFrom(o => o.OrderItems.Sum(i => i.PurchasePrice)))
                .ReverseMap();

            CreateMap<OrderItem, OrderItemMobileDto>().ReverseMap();
            CreateMap<OrderItem, OrderItemDto>().ReverseMap();

            CreateMap<RestaurantPickupStop, RestaurantStopDto>().ReverseMap();
            CreateMap<RestaurantDeliveryStop, RestaurantStopDto>().ReverseMap();
            CreateMap<CreateRestaurantStopDto, RestaurantPickupStop>().ReverseMap();
            CreateMap<CreateRestaurantStopDto, RestaurantDeliveryStop>().ReverseMap();
        }
    }
}