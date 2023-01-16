using System.Threading.Tasks;
using Models.Misc;
using Services.ExternalServices;

namespace Services;

public class PayError
{
    public static PayError CreatePaymentNetworkError = new PayError("01", "Не выполнился запрос к register.do");

    public static PayError NoUsernameOrPassword(long restaurantId) => new PayError("02", $"Не заполнены поля Username и/или Password для ресторана ({restaurantId})");

    public static PayError SberbankGetOrderStatusExtendedUnavailable = new PayError("03", "Фейл запроса getOrderStatusExtended");

    public static PayError SberbankUnknownErrorCode(string errorCode) => new PayError("04", $"Получен невалидный ErrorCode: {errorCode}");

    public static PayError SberbankUnknownOrderStatus(int orderStatus) => new PayError("05", $"Неизвестный OrderStatus: {orderStatus}");
    public static PayError SberbankRegisterErrorCode(string errorCode) => new PayError("06", $"Получен ErrorCode \"{errorCode}\" при создании заказа.");
    public static PayError RestaurantNotFound(long orderId) => new PayError("07", $"Не найден ресторан заказа \"{orderId}\".");


    public string Code { get; set; }

    public string Message { get; set; }

    private PayError(string code, string message)
    {
        Code = code;
        Message = message;
    }

    public async Task Throw(long? clientAccountId, string clientLogin, long? workerId)
    {
        await TelegramAPI.Send("Ошибка обработки оплаты:\n" +
                               $"{Message}\n" +
                               $"ClientAccount: {(clientAccountId?.ToString() ?? "не определён")}\n" +
                               $"ClientLogin: {(clientLogin ?? "не определён")}\n" +
                               $"WorkerId: {(workerId?.ToString() ?? "не определён")}");
        throw new AkianaException(Code);
    }
}