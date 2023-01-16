using System;
using System.Threading.Tasks;
using Models.Misc;
using Services.CommonServices.Abstractions;
using Services.ExternalServices;

namespace Services.CommonServices.Implementations
{
    public class SMSService : ISMSService
    {
        public async Task Send(string text, string recipient)
        {
            var sendSMSResult = await SMSC.SendSMS(recipient, text);

            try
            {
                var status = Convert.ToInt32(sendSMSResult[1]);
                if (status < 0)
                {
                    await TelegramAPI.Send($"An Error Has Occured In SMSService.Send(text: \"{text}\", recipient: \"{recipient}\"): SMSC responded with status ({status})");
                }
            }
            catch
            {
                await TelegramAPI.Send($"An Error Has Occured In SMSService.Send(text: \"{text}\", recipient: \"{recipient}\"): SMSC responded with unparseable status");
            }
        }

        public async Task<(bool result, int code)> SendCall(string recipient)
        {
            try
            {
                var sendSMSResult = await SMSC.SendSMS(recipient, "code", format: 9/*, query: "fmt=3"*/);

                var status = Convert.ToInt32(sendSMSResult[1]);
                if (status < 0)
                {
                    await TelegramAPI.Send($"An Error Has Occured In SMSService.SendCall(recipient: \"{recipient}\"): SMSC responded: \n{string.Join("", sendSMSResult)}");
                    return (false, 0);
                }

                var code = Convert.ToInt32(sendSMSResult[4]);

                return (true, code);
            }
            catch
            {
                await TelegramAPI.Send($"An Error Has Occured In SMSService.SendCall(recipient: \"{recipient}\"): SMSC responded with unparseable status");
                return (false, 0);
            }
        }
    }
}