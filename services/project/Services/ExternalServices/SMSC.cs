// SMSC.RU API (smsc.ru) версия 3.1 (03.07.2019)

using System;
using System.IO;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using System.Web;

// #define SMSC_DEBUG
// #define SMSC_SMTP_LOGIN

namespace Services.ExternalServices
{
    public static class SMSC
    {
        // Константы с параметрами отправки
        private const string SmscLogin = "OOOAkiana"; // логин клиента
        private const string SmscPassword = "e2471abefa9ba8fdcc82ff3cd8b9cb876a9ba384"; // пароль или MD5-хеш пароля в нижнем регистре

        private static bool _usePost = false; // использовать метод POST

        private const bool SmscHttps = false; // использовать HTTPS протокол
        private const string SmscCharset = "utf-8"; // кодировка сообщения (windows-1251 или koi8-r), по умолчанию используется utf-8
        private const bool SmscDebug = false; // флаг отладки

        // Константы для отправки SMS по SMTP
        private const string SmtpFrom = "api@smsc.ru"; // e-mail адрес отправителя
        private const string SmtpServer = "send.smsc.ru"; // адрес smtp сервера
        private const string SmtpLogin = ""; // логин для smtp сервера
        private const string SmtpPassword = ""; // пароль для smtp сервера

        private static string[][] _d2Res;

        // Метод отправки SMS
        //
        // обязательные параметры:
        //
        // phones - список телефонов через запятую или точку с запятой
        // message - отправляемое сообщение
        //
        // необязательные параметры:
        //
        // translit - переводить или нет в транслит
        // time - необходимое время доставки в виде строки (DDMMYYhhmm, h1-h2, 0ts, +m)
        // id - идентификатор сообщения. Представляет собой 32-битное число в диапазоне от 1 до 2147483647.
        // format - формат сообщения (0 - обычное sms, 1 - flash-sms, 2 - wap-push, 3 - hlr, 4 - bin, 5 - bin-hex, 6 - ping-sms, 7 - mms, 8 - mail, 9 - call, 10 - viber, 11 - soc)
        // sender - имя отправителя (Sender ID). Для отключения Sender ID по умолчанию необходимо в качестве имени
        // передать пустую строку или точку.
        // query - строка дополнительных параметров, добавляемая в URL-запрос ("valid=01:00&maxsms=3")
        //
        // возвращает массив строк (<id>, <количество sms>, <стоимость>, <баланс>) в случае успешной отправки
        // либо массив строк (<id>, -<код ошибки>) в случае ошибки
        public static async Task<string[]> SendSMS(string phones, string message, int translit = 0, string time = "", int id = 0, int format = 0, string sender = "", string query = "", string[] files = null)
        {
            if (files != null)
                _usePost = true;

            string[] formats =
            {
                "flash=1", "push=1", "hlr=1", "bin=1", "bin=2", "ping=1", "mms=1", "mail=1", "call=1", "viber=1", "soc=1"
            };

            string[] result = await SMSCSendCmdInternal(
                "send",
                "cost=3&" +
                "phones=" + UrlEncode(phones) +
                "&mes=" + UrlEncode(message) +
                "&id=" + id +
                "&translit=" + translit +
                (format > 0 ? "&" + formats[format - 1] : "") +
                (sender != "" ? "&sender=" + UrlEncode(sender) : "") +
                (time != "" ? "&time=" + UrlEncode(time) : "") +
                (query != "" ? "&" + query : ""),
                files
            );

            // (id, cnt, cost, balance) или (id, -error)
#if SMSC_DEBUG
            if (SmscDebug)
            {
                if (Convert.ToInt32(result[1]) > 0)
                    PrintDebug("Сообщение отправлено успешно. ID: " + result[0] + ", всего SMS: " + result[1] + ", стоимость: " + result[2] + ", баланс: " + result[3]);
                else
                {
                    PrintDebug("Ошибка №" + result[1].Substring(1, 1) + (result[0] != "0" ? ", ID: " + result[0] : ""));
                }

                PrintDebug(string.Join("\n", result));
            }
#endif

            return result;
        }

        // SMTP версия метода отправки SMS

        public static void SendSMSMail(string phones, string message, int translit = 0, string time = "", int id = 0, int format = 0, string sender = "")
        {
            var mailMessage = new MailMessage();

            mailMessage.To.Add("send@send.smsc.ru");
            mailMessage.From = new MailAddress(SmtpFrom, "");

            mailMessage.Body = SmscLogin + ":" + SmscPassword + ":" + id + ":" + time + ":"
                               + translit + "," + format + "," + sender
                               + ":" + phones + ":" + message;

            mailMessage.BodyEncoding = Encoding.GetEncoding(SmscCharset);
            mailMessage.IsBodyHtml = false;

            var client = new SmtpClient(SmtpServer, 25)
            {
                DeliveryMethod = SmtpDeliveryMethod.Network,
                EnableSsl = false,
                UseDefaultCredentials = false
            };

#if SMSC_SMTP_LOGIN
            if (SmtpLogin != "")
                client.Credentials = new NetworkCredential(SmtpLogin, SmtpPassword);
#endif

            client.Send(mailMessage);
        }

        // Метод получения стоимости SMS
        //
        // обязательные параметры:
        //
        // phones - список телефонов через запятую или точку с запятой
        // message - отправляемое сообщение 
        //
        // необязательные параметры:
        //
        // translit - переводить или нет в транслит
        // format - формат сообщения (0 - обычное sms, 1 - flash-sms, 2 - wap-push, 3 - hlr, 4 - bin, 5 - bin-hex, 6 - ping-sms, 7 - mms, 8 - mail, 9 - call, 10 - viber, 11 - soc)
        // sender - имя отправителя (Sender ID)
        // query - строка дополнительных параметров, добавляемая в URL-запрос ("list=79999999999:Ваш пароль: 123\n78888888888:Ваш пароль: 456")
        //
        // возвращает массив (<стоимость>, <количество sms>) либо массив (0, -<код ошибки>) в случае ошибки

        public static async Task<string[]> GetSMSCost(string phones, string message, int translit = 0, int format = 0, string sender = "", string query = "")
        {
            string[] formats = {"flash=1", "push=1", "hlr=1", "bin=1", "bin=2", "ping=1", "mms=1", "mail=1", "call=1", "viber=1", "soc=1"};

            var result = await SMSCSendCmdInternal(
                "send",
                "cost=1" +
                "&phones=" + UrlEncode(phones) +
                "&mes=" + UrlEncode(message) +
                translit + (format > 0 ? "&" + formats[format - 1] : "") +
                (sender != "" ? "&sender=" + UrlEncode(sender) : "") +
                (query != "" ? "&query" : "")
            );

            // (cost, cnt) или (0, -error)
#if SMSC_DEBUG
            if (SmscDebug)
            {
                if (Convert.ToInt32(result[1]) > 0)
                    PrintDebug("Стоимость рассылки: " + result[0] + ". Всего SMS: " + result[1]);
                else
                {
                    PrintDebug("Ошибка №" + result[1].Substring(1, 1));
                }
            }
#endif

            return result;
        }

        // Метод проверки статуса отправленного SMS или HLR-запроса
        //
        // id - ID cообщения или список ID через запятую
        // phone - номер телефона или список номеров через запятую
        // all - вернуть все данные отправленного SMS, включая текст сообщения (0,1 или 2)
        //
        // возвращает массив (для множественного запроса возвращается массив с единственным элементом, равным 1. В этом случае статусы сохраняются в
        //					двумерном динамическом массиве класса D2Res):
        //
        // для одиночного SMS-сообщения:
        // (<статус>, <время изменения>, <код ошибки доставки>)
        //
        // для HLR-запроса:
        // (<статус>, <время изменения>, <код ошибки sms>, <код IMSI SIM-карты>, <номер сервис-центра>, <код страны регистрации>, <код оператора>,
        // <название страны регистрации>, <название оператора>, <название роуминговой страны>, <название роумингового оператора>)
        //
        // при all = 1 дополнительно возвращаются элементы в конце массива:
        // (<время отправки>, <номер телефона>, <стоимость>, <sender id>, <название статуса>, <текст сообщения>)
        //
        // при all = 2 дополнительно возвращаются элементы <страна>, <оператор> и <регион>
        //
        // при множественном запросе (данные по статусам сохраняются в двумерном массиве D2Res):
        // если all = 0, то для каждого сообщения или HLR-запроса дополнительно возвращается <ID сообщения> и <номер телефона>
        //
        // если all = 1 или all = 2, то в ответ добавляется <ID сообщения>
        //
        // либо массив (0, -<код ошибки>) в случае ошибки

        public static async Task<string[]> GetStatus(string id, string phone, int all = 0)
        {
            string[] result = await SMSCSendCmdInternal(
                "status",
                "phone=" + UrlEncode(phone) +
                "&id=" + UrlEncode(id) +
                "&all=" + all
            );

            // (status, time, err, ...) или (0, -error)

            if (id.IndexOf(',') == -1)
            {
#if SMSC_DEBUG
                if (SmscDebug)
                {
                    if (result[1] != "" && Convert.ToInt32(result[1]) >= 0)
                    {
                        int timestamp = Convert.ToInt32(result[1]);
                        DateTime offset = new DateTime(1970, 1, 1, 0, 0, 0, 0);
                        DateTime date = offset.AddSeconds(timestamp);

                        PrintDebug("Статус SMS = " + result[0] + (timestamp > 0 ? ", время изменения статуса - " + date.ToLocalTime() : ""));
                    }
                    else
                    {
                        PrintDebug("Ошибка №" + result[1].Substring(1, 1));
                    }
                }
#endif

                int idx = all == 1 ? 9 : 12;

                if (all > 0 && result.Length > idx && (result.Length < idx + 5 || result[idx + 5] != "HLR"))
                    result = String.Join(",", result).Split(",".ToCharArray(), idx);
            }
            else
            {
                if (result.Length == 1 && result[0].IndexOf('-') == 2)
                    return result[0].Split(',');

                Array.Resize(ref _d2Res, 0);
                Array.Resize(ref _d2Res, result.Length);

                for (int i = 0; i < _d2Res.Length; i++)
                    _d2Res[i] = result[i].Split(',');

                Array.Resize(ref result, 1);
                result[0] = "1";
            }

            return result;
        }

        // Метод получения баланса
        //
        // без параметров
        //
        // возвращает баланс в виде строки или пустую строку в случае ошибки

        public static async Task<string> GetBalance()
        {
            var result = await SMSCSendCmdInternal("balance", ""); // (balance) или (0, -error)

#if SMSC_DEBUG
            if (SmscDebug)
            {
                if (result.Length == 1)
                    PrintDebug("Сумма на счете: " + result[0]);
                else
                    PrintDebug("Ошибка №" + result[1].Substring(1, 1));
            }
#endif

            return result.Length == 1 ? result[0] : "";
        }

        // ПРИВАТНЫЕ МЕТОДЫ

        // Метод вызова запроса. Формирует URL и делает 3 попытки чтения

        private static async Task<string[]> SMSCSendCmdInternal(string command, string arg, string[] files = null)
        {
            string url, _url;

            arg = "login=" + UrlEncode(SmscLogin) + "&psw=" + UrlEncode(SmscPassword) + "&fmt=1&charset=" + SmscCharset + "&" + arg;

            url = _url = (SmscHttps ? "https" : "http") + "://smsc.ru/sys/" + command + ".php" + (_usePost ? "" : "?" + arg);

            string result;
            int i = 0;
            HttpWebRequest request;
            StreamReader sr;
            HttpWebResponse response;

            do
            {
                if (i++ > 0)
                    url = _url.Replace("smsc.ru/", "www" + i + ".smsc.ru/");

                request = (HttpWebRequest)WebRequest.Create(url);

                if (_usePost)
                {
                    request.Method = "POST";

                    string postHeader, boundary = "----------" + DateTime.Now.Ticks.ToString("x");
                    byte[] postHeaderBytes, boundaryBytes = Encoding.ASCII.GetBytes("--" + boundary + "--\r\n"), tbuf;
                    StringBuilder sb = new StringBuilder();
                    int bytesRead;

                    byte[] output = new byte[0];

                    if (files == null)
                    {
                        request.ContentType = "application/x-www-form-urlencoded";
                        output = Encoding.UTF8.GetBytes(arg);
                        request.ContentLength = output.Length;
                    }
                    else
                    {
                        request.ContentType = "multipart/form-data; boundary=" + boundary;

                        string[] parameter = arg.Split('&');
                        int filesLength = files.Length;

                        for (int parameterIndex = 0; parameterIndex < parameter.Length + filesLength; parameterIndex++)
                        {
                            sb.Clear();

                            sb.Append("--");
                            sb.Append(boundary);
                            sb.Append("\r\n");
                            sb.Append("Content-Disposition: form-data; name=\"");

                            bool pof = parameterIndex < filesLength;
                            var nv = Array.Empty<string>();

                            if (pof)
                            {
                                sb.Append("File" + (parameterIndex + 1));
                                sb.Append("\"; filename=\"");
                                sb.Append(Path.GetFileName(files[parameterIndex]));
                            }
                            else
                            {
                                nv = parameter[parameterIndex - filesLength].Split('=');
                                sb.Append(nv[0]);
                            }

                            sb.Append("\"");
                            sb.Append("\r\n");
                            sb.Append("Content-Type: ");
                            sb.Append(pof ? "application/octet-stream" : "text/plain; charset=\"" + SmscCharset + "\"");
                            sb.Append("\r\n");
                            sb.Append("Content-Transfer-Encoding: binary");
                            sb.Append("\r\n");
                            sb.Append("\r\n");

                            postHeader = sb.ToString();
                            postHeaderBytes = Encoding.UTF8.GetBytes(postHeader);

                            output = ConcatBytes(output, postHeaderBytes);

                            if (pof)
                            {
                                var fileStream = new FileStream(files[parameterIndex], FileMode.Open, FileAccess.Read);

                                // Write out the file contents
                                var buffer = new Byte[checked((uint)Math.Min(4096, (int)fileStream.Length))];

                                bytesRead = 0;
                                while ((bytesRead = fileStream.Read(buffer, 0, buffer.Length)) != 0)
                                {
                                    tbuf = buffer;
                                    Array.Resize(ref tbuf, bytesRead);

                                    output = ConcatBytes(output, tbuf);
                                }
                            }
                            else
                            {
                                var vl = Encoding.UTF8.GetBytes(nv[1]);
                                output = ConcatBytes(output, vl);
                            }

                            output = ConcatBytes(output, Encoding.UTF8.GetBytes("\r\n"));
                        }

                        output = ConcatBytes(output, boundaryBytes);

                        request.ContentLength = output.Length;
                    }

                    var requestStream = request.GetRequestStream();
                    await requestStream.WriteAsync(output.AsMemory(0, output.Length));
                }

                try
                {
                    response = (HttpWebResponse)await request.GetResponseAsync();

                    sr = new StreamReader(response.GetResponseStream());
                    result = await sr.ReadToEndAsync();
                }
                catch (WebException)
                {
                    result = "";
                }
            } while (result == "" && i < 5);

            if (result == "")
            {
#if SMSC_DEBUG
                if (SmscDebug)
                    PrintDebug("Ошибка чтения адреса: " + url);
#endif
                result = ","; // фиктивный ответ
            }

            char delimiter = ',';

            if (command == "status")
            {
                string[] parameters = arg.Split('&');

                for (i = 0; i < parameters.Length; i++)
                {
                    string[] parameterSplit = parameters[i].Split('=', 2);

                    if (parameterSplit[0] == "id" && parameterSplit[1].IndexOf("%2c") > 0) // запятая в id - множественный запрос
                        delimiter = '\n';
                }
            }

            return result.Split(delimiter);
        }

        // кодирование параметра в http-запросе
        private static string UrlEncode(string str)
        {
            if (_usePost) return str;

            return HttpUtility.UrlEncode(str);
        }

        // объединение байтовых массивов
        private static byte[] ConcatBytes(byte[] firstArray, byte[] secondArray)
        {
            int firstArrayLength = firstArray.Length;

            Array.Resize(ref firstArray, firstArray.Length + secondArray.Length);
            Array.Copy(secondArray, 0, firstArray, firstArrayLength, secondArray.Length);

            return firstArray;
        }

        // вывод отладочной информации
        private static void PrintDebug(string str)
        {
            Console.WriteLine(str);
            // System.Windows.Forms.MessageBox.Show(str);
        }
    }
}

// Examples:
// SMSC smsc = new SMSC();
// string[] r = smsc.send_sms("79999999999", "Ваш пароль: 123", 2);
// string[] r = smsc.send_sms("79999999999", "http://smsc.ru\nSMSC.RU", 0, "", 0, 0, "", "maxsms=3");
// string[] r = smsc.send_sms("79999999999", "0605040B8423F0DC0601AE02056A0045C60C036D79736974652E72750001036D7973697465000101", 0, "", 0, 5);
// string[] r = smsc.send_sms("79999999999", "", 0, "", 0, 3);
// string[] r = smsc.send_sms("dest@mysite.com", "Ваш пароль: 123", 0, 0, 0, 8, "source@mysite.com", "subj=Confirmation");
// string[] r = smsc.get_sms_cost("79999999999", "Вы успешно зарегистрированы!");
// smsc.send_sms_mail("79999999999", "Ваш пароль: 123", 0, "0101121000");
// string[] r = smsc.get_status("12345", "79999999999");
// string balance = smsc.get_balance();