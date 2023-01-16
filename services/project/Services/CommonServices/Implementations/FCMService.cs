using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FirebaseAdmin;
using FirebaseAdmin.Messaging;
using Google.Apis.Auth.OAuth2;
using Models.DTOs.MobilePushes;
using Services.CommonServices.Abstractions;
using Services.ExternalServices;

namespace Services.CommonServices.Implementations
{
    public class FCMService : IFCMService
    {
        // For ProjectId look in firebase project overview
        private const string ProjectId = "akiana-f3e10";
        private const string ImagesUrl = "https://akiana.io/images/MobilePush/";

        private async Task<string> SendAll(IEnumerable<Message> messages)
        {
            try
            {
                if (FirebaseApp.DefaultInstance is null)
                {
                    FirebaseApp.Create(new AppOptions
                    {
                        Credential = await GoogleCredential.GetApplicationDefaultAsync()
                    });
                }

                var messaging = FirebaseMessaging.GetMessaging(FirebaseApp.DefaultInstance);

                var batchResponse = await messaging.SendAllAsync(messages);

                return string.Join('\n', batchResponse.Responses.Select(r =>
                    r.IsSuccess ? r.MessageId : r.Exception.Message)
                );
            }
            catch (Exception ex)
            {
                await TelegramAPI.Send($"FCMService.SendAll Failed:\n```{ex.Message}```");
                return "";
            }
        }

        public async Task<string> SendByCities(CreateMobilePushDto createMobilePushDto)
        {
            var result = await SendAll(createMobilePushDto.Targets.Select(targetDto => new Message()
            {
                Topic = $"/topics/CITY_{targetDto.Id}",
                Notification = new Notification
                {
                    Title = createMobilePushDto.Title,
                    Body = createMobilePushDto.Content,
                    ImageUrl = string.IsNullOrEmpty(createMobilePushDto.Image) ? null : ImagesUrl + createMobilePushDto.Image
                }
            }));

            return result;
        }

        public async Task<string> SendByPriceGroups(CreateMobilePushDto createMobilePushDto)
        {
            var result = await SendAll(createMobilePushDto.Targets.Select(targetDto => new Message()
            {
                Topic = $"/topics/PRICE_GROUP_{targetDto.Id}",
                Notification = new Notification
                {
                    Title = createMobilePushDto.Title,
                    Body = createMobilePushDto.Content,
                    ImageUrl = string.IsNullOrEmpty(createMobilePushDto.Image) ? null : ImagesUrl + createMobilePushDto.Image
                }
            }));

            return result;
        }
    }
}