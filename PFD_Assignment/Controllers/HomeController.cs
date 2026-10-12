using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using PFD_Assignment.Models;
using System.Diagnostics;
using FaceRecognitionDotNet;
using PFD_Assignment.DAL;
using System.Drawing;
using System.Text.RegularExpressions;
using System.Globalization;
using Twilio;
using Twilio.Rest.Api.V2010.Account;
using Twilio.Types;
using Twilio.Http;
using System.Runtime.InteropServices;
using Microsoft.AspNetCore.Hosting;
using System.Text.Json;
using Google.Cloud.Speech.V1;
using Google.Protobuf;
using Newtonsoft.Json;
using System.Configuration;
using Google.Apis.Auth.OAuth2;
using System.Threading.Channels;

namespace PFD_Assignment.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly FaceRecognition _faceRecognition;
        FeedbackDAL feedbackDAL = new FeedbackDAL();
        LoginDAL loginContext = new LoginDAL();
        FeatureDAL featureContext = new FeatureDAL();
        TransactionDAL transactionContext = new TransactionDAL();
        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
            // Initialize FaceRecognition. Adjust the directory path as necessary.
            // Assuming 'facemodels' is directly under the 'wwwroot' folder
            var modelsDirectory = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "facemodels");

            try
            {
                _faceRecognition = FaceRecognition.Create(modelsDirectory);
            }
            catch
            (Exception ex)
            {
            }
        }
        static byte[] ImageToByteArray(string imagePath)
        {
            // Convert image to byte array
            using (FileStream fs = new FileStream(imagePath, FileMode.Open, FileAccess.Read))
            {
                using (MemoryStream ms = new MemoryStream())
                {
                    fs.CopyTo(ms);
                    return ms.ToArray();
                }
            }
        }

        static void SaveByteArrayToFile(string fileName, byte[] byteArray)
        {
            using (FileStream fs = new FileStream(fileName, FileMode.Create, FileAccess.Write))
            {
                fs.Write(byteArray, 0, byteArray.Length);
            }
        }

        public IActionResult SelectLanguages()
        {
            // Personal face-photo seeding was removed for the published archive.
            return View();
        }

        public IActionResult SelectLoginMethod()
        {
            return View();
        }

        public IActionResult FaceAuthentication()
        {
            return View();
        }

        public IActionResult PasswordPIN()
        {
            return View();
        }

        public IActionResult FingerprintAuthentication()
        {
            return View();
        }

        private readonly Dictionary<string, string> _featureToActionMap = new Dictionary<string, string>
    {
        { "Account Services", "AccountServices1" },
        { "Card Services", "CardServices1" },
    };

        public IActionResult HomepageCashWithdraw()
        {
            var customer = HttpContext.Session.GetObject<Customer>("Customer");
            var account = HttpContext.Session.GetObject<Account>("Account");
            var mostRecentFeatureName = featureContext.GetMostRecentFeatureUsageByCustomerId(customer.CustomerID)?.FeatureName;

            ViewBag.Customer = customer;
            ViewBag.Account = account;

            string mostRecentActionName = null;
            if (mostRecentFeatureName != null && _featureToActionMap.TryGetValue(mostRecentFeatureName, out var actionName))
            {
                mostRecentActionName = actionName;
            }
            ViewBag.MostRecentActionName = mostRecentActionName;
            ViewBag.MostRecentFeatureName = mostRecentFeatureName;

            // Favourite
            var favouriteFeature = featureContext.GetFavoriteFeatureByCustomerId(customer.CustomerID);
            ViewBag.FavouriteFeatureName = favouriteFeature?.FeatureName;

            // Action name
            string favouriteActionName = null;
            if (favouriteFeature != null && _featureToActionMap.TryGetValue(favouriteFeature.FeatureName, out var actionName2))
            {
                favouriteActionName = actionName2;
            }

            ViewBag.FavouriteActionName = favouriteActionName;

            return View();
        }

        public IActionResult OtherServices1()
        {
            return View();
        }

        public IActionResult AccountServices1()
        {
            var customer = HttpContext.Session.GetObject<Customer>("Customer");
            if (customer != null)
            {
                featureContext.UpdateFeatureUsage("Account Services", customer.CustomerID);
            }

            return View();
        }

        public IActionResult CardServices1()
        {
            var customer = HttpContext.Session.GetObject<Customer>("Customer");
            if (customer != null)
            {
                featureContext.UpdateFeatureUsage("Card Services", customer.CustomerID);
            }
            return View();
        }

        public IActionResult TransferFunds()
        {
            return View();
        }

        public IActionResult Receipt()
        {
            return View();
        }

        public IActionResult Thankyou()
        {
            return View();
        }

        public IActionResult AmountWithdraw()
        {
            return View();
        }

        public IActionResult OtherCashAmount()
        {
            return View();
        }

        public IActionResult FeedbackStarSelection()
        {
            return View();
        }

        public IActionResult QRCodeFeedback()
        {

            return View();
        }

        public IActionResult feedback1()
        {
            var transactionid = HttpContext.Session.GetInt32("TransactionID");
            feedbackDAL.updatestarfeedback(1, (int)transactionid);
            return View("FeedBackCategorySelection");
        }

        public IActionResult feedback2()
        {
            var transactionid = HttpContext.Session.GetInt32("TransactionID");
            feedbackDAL.updatestarfeedback(2, (int)transactionid);
            return View("FeedBackCategorySelection");
        }

        public IActionResult feedback3()
        {
            var transactionid = HttpContext.Session.GetInt32("TransactionID");
            feedbackDAL.updatestarfeedback(3, (int)transactionid);
            return View("FeedBackCategorySelection");
        }

        public IActionResult feedback4()
        {
            var transactionid = HttpContext.Session.GetInt32("TransactionID");
            feedbackDAL.updatestarfeedback(4, (int)transactionid);
            return View("Thankyou");
        }

        public IActionResult feedback5()
        {
            var transactionid = HttpContext.Session.GetInt32("TransactionID");
            feedbackDAL.updatestarfeedback(5, (int)transactionid);
            return View("Thankyou");
        }

        public IActionResult FeedBackCategorySelection()
        {
            return View();
        }

        public IActionResult feedbackcategory1()
        {
            var transactionid = HttpContext.Session.GetInt32("TransactionID");
            feedbackDAL.updatecategoryfeedback("Services Provided", (int)transactionid);
            return RedirectToAction("QRCodeFeedback");

        }

        public IActionResult feedbackcategory2()
        {
            var transactionid = HttpContext.Session.GetInt32("TransactionID");
            feedbackDAL.updatecategoryfeedback("Ease of Usage", (int)transactionid);
            return RedirectToAction("QRCodeFeedback");
        }

        public IActionResult feedbackcategory3()
        {
            var transactionid = HttpContext.Session.GetInt32("TransactionID");
            feedbackDAL.updatecategoryfeedback("ATM Usability", (int)transactionid);
            return RedirectToAction("QRCodeFeedback");
        }

        public IActionResult feedbackcategory4()
        {
            var transactionid = HttpContext.Session.GetInt32("TransactionID");
            feedbackDAL.updatecategoryfeedback("ATM Security", (int)transactionid);
            return RedirectToAction("QRCodeFeedback");
        }

        [HttpPost]
        public IActionResult OtherCashWithdraw(int withdrawalAmount)
        {
            if (withdrawalAmount <= 0)
            {
                // Handle error: withdrawal amount must be positive
                ModelState.AddModelError(string.Empty, "Invalid amount. Please enter a positive number.");
                return View("OtherCashAmount"); // Return to the form view
            }

            var account = HttpContext.Session.GetObject<Account>("Account");

            if (account == null)
            {
                // Handle error: account not found in session
                return RedirectToAction("Error");
            }

            if (account.Amount < withdrawalAmount)
            {
                // Handle insufficient funds
                ModelState.AddModelError(string.Empty, "Insufficient funds.");
                return View("OtherCashAmount"); // Return to the form view
            }

            // Perform withdrawal
            account.Amount -= withdrawalAmount; // Deduct amount from account object
            HttpContext.Session.SetObject("Account", account); // Update session

            bool updateSuccess = transactionContext.UpdateAccountBalance(account.AccountNum, account.Amount);
            int transactionSuccess = transactionContext.writeTransaction(account.AccountNum, account.Amount, withdrawalAmount, "S$" + withdrawalAmount + " has been withdrawn from the account.");
            if (!updateSuccess && transactionSuccess != -1)
            {
                // Handle error in updating the database
                return RedirectToAction("Error");
            }
            HttpContext.Session.SetInt32("TransactionID", transactionSuccess);
            HttpContext.Session.SetInt32("WithdrawnAmount", withdrawalAmount);
            // Redirect to a success page or similar
            return RedirectToAction("Receipt");
        }

        public IActionResult OtherOCBCAccount()
        {
            var customer = HttpContext.Session.GetObject<Customer>("Customer");
            var account = HttpContext.Session.GetObject<Account>("Account");

            // Generate OTP
            GenerateAndSendOTP(customer.Phone);
            ViewBag.Customer = customer;
            ViewBag.Account = account;
            return View();
        }

        public IActionResult AccountBalance()
        {
            var customer = HttpContext.Session.GetObject<Customer>("Customer");
            var account = HttpContext.Session.GetObject<Account>("Account");

            ViewBag.Customer = customer;
            ViewBag.Account = account;
            return View();
        }

        public IActionResult SetLanguage(string culture, string returnUrl)
        {
            Response.Cookies.Append(
                CookieRequestCultureProvider.DefaultCookieName,
                CookieRequestCultureProvider.MakeCookieValue(new RequestCulture(culture)),
                new CookieOptions { Expires = DateTimeOffset.UtcNow.AddYears(1) }
            );

            return LocalRedirect(returnUrl); // Redirect back to the returnUrl
        }

        private System.Drawing.Image ConvertBase64ToImage(string base64String)
        {
            // Convert base64 string to byte[]
            byte[] imageBytes = Convert.FromBase64String(base64String);
            // Convert byte[] to Image
            using (var ms = new MemoryStream(imageBytes))
            {
                return System.Drawing.Image.FromStream(ms, true);
            }
        }

        private FaceEncoding GetFaceEncoding(System.Drawing.Image image)
        {
            using (var bitmap = new Bitmap(image)) // Create a new Bitmap from the original image
            {
                using (var ms = new MemoryStream())
                {
                    bitmap.Save(ms, System.Drawing.Imaging.ImageFormat.Png); // Save Bitmap to MemoryStream
                    ms.Seek(0, SeekOrigin.Begin); // Reset memory stream position

                    var frImage = FaceRecognition.LoadImage(bitmap); // Use Bitmap with LoadImage

                    var faceLocations = _faceRecognition.FaceLocations(frImage).ToList(); // Convert to List
                    if (faceLocations.Any())
                    {
                        // Assuming you're only interested in the first face found
                        return _faceRecognition.FaceEncodings(frImage, new List<Location> { faceLocations.First() }).FirstOrDefault();
                    }
                }
            }

            return null; // or handle no face found scenario
        }

        [HttpPost]
        public IActionResult OtherOCBCAccount(string phoneNumber, decimal depositAmount)
        {
            var account = HttpContext.Session.GetObject<Account>("Account");
            var customer = HttpContext.Session.GetObject<Customer>("Customer");

            if (depositAmount <= 0)
            {
                ModelState.AddModelError(string.Empty, "Invalid amount. Please enter a positive number.");
                GenerateAndSendOTP(customer.Phone);
                return View();
            }

            if (account.Amount < depositAmount)
            {
                ModelState.AddModelError(string.Empty, "Insufficient funds.");
                GenerateAndSendOTP(customer.Phone);

                return View();
            }

            string recipientAccountNumber = transactionContext.GetAccountNumberByPhoneNumber(phoneNumber);
            if (string.IsNullOrEmpty(recipientAccountNumber))
            {
                ModelState.AddModelError(string.Empty, "Invalid phone number.");
                GenerateAndSendOTP(customer.Phone);
                return View();
            }

            bool transferSuccess = transactionContext.TransferFunds(account.AccountNum, recipientAccountNumber, depositAmount);
            int transactionSuccess = transactionContext.writeTransaction(account.AccountNum, account.Amount -= depositAmount, depositAmount, "S$" + depositAmount + " has been deposited into account " + recipientAccountNumber + ".");
            if (!transferSuccess && transactionSuccess != -1)
            {
                ModelState.AddModelError(string.Empty, "Transfer failed.");
                return View();
            }

            // Update the local account object to reflect the new balance
            account.Amount -= depositAmount;

            // Update session with the modified account object
            HttpContext.Session.SetObject("Account", account);

            HttpContext.Session.SetInt32("TransactionID", transactionSuccess);

            // Redirect to a success page or similar
            return RedirectToAction("FeedbackStarSelection");
        }

        private System.Drawing.Image ConvertByteArrayToImage(byte[] byteArray)
        {
            using (var ms = new MemoryStream(byteArray))
            {
                return System.Drawing.Image.FromStream(ms);
            }
        }

        [HttpPost]
        public async Task<IActionResult> ReceiveFaceData(string image)
        {
            if (string.IsNullOrWhiteSpace(image))
            {
                return Json(new { success = false, message = "Base64 string is null or whitespace." });
            }

            try
            {
                // Ensure the string is URL-safe by replacing spaces with '+'
                // The client side should send the Base64 string without the "data:image/png;base64," prefix.
                // If it includes the prefix, you need to remove it before conversion.
                if (image.StartsWith("data:image/png;base64,"))
                {
                    image = image.Replace("data:image/png;base64,", "");
                }

                var faceImage = ConvertBase64ToImage(image);

                if (faceImage == null)
                {
                    return Json(new { success = false, message = "Face image could not be decoded from Base64 string." });
                }

                var unknownFaceEncoding = GetFaceEncoding(faceImage);
                if (unknownFaceEncoding == null)
                {
                    return Json(new { success = false, message = "Unknown face encoding could not be retrieved." });
                }

                // Fetch all face images from database
                var allFaces = loginContext.GetAllFaceImages();

                foreach (var dbFace in allFaces)
                {
                    var dbFaceImage = ConvertByteArrayToImage(dbFace.FaceImage);
                    var knownFaceEncoding = GetFaceEncoding(dbFaceImage);

                    if (knownFaceEncoding == null)
                    {
                        continue; // Skip if encoding could not be retrieved
                    }

                    // Compare Faces
                    bool match = FaceRecognition.CompareFace(knownFaceEncoding, unknownFaceEncoding, 0.4); // Adjust tolerance as needed

                    if (match)
                    {
                        // Face matches. Authentication successful.
                        var account = loginContext.GetAccountByNumber(dbFace.AccountNum);
                        var customer = loginContext.GetCustomerByAccountNumber(dbFace.AccountNum);
                        HttpContext.Session.SetObject("Account", account);
                        HttpContext.Session.SetObject("Customer", customer);
                        return Json(new { success = true, redirectUrl = Url.Action("HomepageCashWithdraw", "Home") });
                    }
                }

                // No match found. Authentication failed.
                return Json(new { success = false, message = "Face not recognized" });
            }
            catch (FormatException fe)
            {
                return Json(new { success = false, message = "Invalid Base64 format." });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Error in processing face data" });
            }
        }

        [HttpPost]
        public IActionResult PasswordPIN(string pin)
        {
            if (string.IsNullOrWhiteSpace(pin) || !Regex.IsMatch(pin, @"^\d{6}$"))
            {
                // Handle invalid PIN input
                ModelState.AddModelError("pin", "Invalid PIN. Please enter a 6-digit PIN.");
                return View();
            }

            // Authenticate the user by checking the PIN against the database
            string accountNumber = loginContext.GetAccountNumByPIN(pin);

            if (accountNumber != null)
            {
                // User is authenticated, retrieve customer and account details
                var customer = loginContext.GetCustomerByAccountNumber(accountNumber);
                var account = loginContext.GetAccountByNumber(accountNumber);

                HttpContext.Session.SetObject("Account", account);
                HttpContext.Session.SetObject("Customer", customer);

                return RedirectToAction("HomepageCashWithdraw");
            }
            else
            {
                // Authentication failed, display an error message
                ModelState.AddModelError("pin", "Authentication failed. Please check your PIN.");
                return View();
            }
        }
        public IActionResult ConfirmWithdrawal(int amount)
        {
            var account = HttpContext.Session.GetObject<Account>("Account");

            if (account == null)
            {
                // Handle error: account not found in session
                return RedirectToAction("Error");
            }

            if (account.Amount < amount)
            {
                // Handle insufficient funds
                TempData["ErrorMessage"] = "Insufficient funds.";
                return RedirectToAction("AmountWithdraw", new { amount = amount });
            }

            // Perform withdrawal
            account.Amount -= amount; // Deduct amount from account object
            HttpContext.Session.SetObject("Account", account); // Update session

            bool updateSuccess = transactionContext.UpdateAccountBalance(account.AccountNum, account.Amount);
            int transactionSuccess = transactionContext.writeTransaction(account.AccountNum, account.Amount, amount, "S$" + amount + " has been withdrawn from the account.");
            if (!updateSuccess && transactionSuccess != -1)
            {
                // Handle error in updating the database
                return RedirectToAction("Error");
            }
            HttpContext.Session.SetInt32("WithdrawnAmount", amount);
            HttpContext.Session.SetInt32("TransactionID", transactionSuccess);
            // Redirect to a success page or similar
            return RedirectToAction("Receipt");
        }

        private async Task SendReceiptSMS(string phoneNumber, int? amount)
        {
            if (!phoneNumber.StartsWith("+65"))
            {
                phoneNumber = "+65" + phoneNumber;
            }
            string accountSid = Environment.GetEnvironmentVariable("ATM_TWILIO_SID") ?? throw new InvalidOperationException("SMS is not configured in this archive."); // Fetch from config
            string authToken = Environment.GetEnvironmentVariable("ATM_TWILIO_TOKEN") ?? throw new InvalidOperationException("SMS is not configured in this archive.");
            TwilioClient.Init(accountSid, authToken);

            var messageOptions = new CreateMessageOptions(new PhoneNumber(phoneNumber))
            {
                From = new PhoneNumber(Environment.GetEnvironmentVariable("ATM_TWILIO_FROM") ?? ""), // Your Twilio number
                Body = $"Receipt for OCBC ATM withdrawal: Amount - {amount}"
            };

            await MessageResource.CreateAsync(messageOptions);
        }

        public async Task<IActionResult> EReceipt()
        {
            if (Environment.GetEnvironmentVariable("ATM_ENABLE_SMS") != "true")
                throw new InvalidOperationException("SMS sending is disabled in the source archive.");

            var customer = HttpContext.Session.GetObject<Customer>("Customer");
            if (customer == null)
            {
                // Handle error: customer not found in session
                return RedirectToAction("Error");
            }

            int? amountWithdrawn = HttpContext.Session.GetInt32("WithdrawnAmount");

            // Send SMS
            await SendReceiptSMS(customer.Phone, amountWithdrawn);

            return View("FeedbackStarSelection"); // Redirect to a confirmation view
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public void GenerateAndSendOTP(string customerPhone)
        {
            var rng = new Random();
            var otp = rng.Next(100000, 999999).ToString(); // Generate a 6-digit OTP

            if (!string.IsNullOrEmpty(customerPhone))
            {
                SendOTP(customerPhone, otp); // Send OTP to the customer's phone
                HttpContext.Session.SetString("OTP", otp); // Store OTP in the session for later verification
            }
        }
        private void SendOTP(string phoneNumber, string otp)
        {
            if (Environment.GetEnvironmentVariable("ATM_ENABLE_SMS") != "true")
                throw new InvalidOperationException("SMS sending is disabled in the source archive.");

            if (!phoneNumber.StartsWith("+65"))
            {
                phoneNumber = "+65" + phoneNumber;
            }
            // Read Twilio settings from appsettings.json
            string accountSid = Environment.GetEnvironmentVariable("ATM_TWILIO_SID") ?? throw new InvalidOperationException("SMS is not configured in this archive."); // Fetch from config
            string authToken = Environment.GetEnvironmentVariable("ATM_TWILIO_TOKEN") ?? throw new InvalidOperationException("SMS is not configured in this archive.");
            TwilioClient.Init(accountSid, authToken);

            var messageOptions = new CreateMessageOptions(
                  new PhoneNumber(phoneNumber));
            messageOptions.From = new PhoneNumber(Environment.GetEnvironmentVariable("ATM_TWILIO_FROM") ?? "");
            messageOptions.Body = $"Your OTP is: {otp}";


            var message = MessageResource.Create(messageOptions);
        }

        [HttpPost]
        public IActionResult VerifyOTP([FromBody] JsonElement data)
        {
            var enteredOtp = data.GetProperty("enteredOtp").GetString()?.Trim(); // Trim to remove any leading/trailing spaces
            var otp = HttpContext.Session.GetString("OTP")?.Trim(); // Also trim the OTP from session

            if (otp == null)
            {
                return Json(new { success = false, message = "OTP session expired or not set." });
            }

            if (otp.Equals(enteredOtp))
            {
                HttpContext.Session.Remove("OTP");
                return Json(new { success = true });
            }
            else
            {
                return Json(new { success = false, message = "Incorrect OTP." });
            }
        }
    }
}