# ATM interaction prototype

An academic ASP.NET Core MVC prototype developed for the OCBC hackathon challenge and extended for Ngee Ann Polytechnic's Portfolio Development Assignment 2 in 2023–24. It explores ATM interactions using fictional accounts. It was not deployed by OCBC and is not a banking product.

## Kelvin Beh's contribution

- Led the team, divided work around teammates' strengths and arranged integration check-ins.
- Implemented voice-based language selection using the browser Web Speech API.
- Added favourite and most-recent feature shortcuts backed by SQL usage records.
- Implemented the peer-to-peer OTP interaction and improved the prototype's face-matching integration.
- Helped integrate the wider application.

The team presentation attributes interface design and wireframes to Koo Bing En, chatbot work to Koo Bing En and Ng Qian Zhe, database/translation work to Ng Qian Zhe, feedback work to Mikhail Adam Zahadi, and mobile transaction pre-setting to Mikhail Adam Zahadi and Dayakar Vikram Sivakumar. These contributions remain shared team work. Kelvin's face-matching work uses a pretrained library; it is not an independently trained face-recognition model.

## Source guide

| Feature | Source |
| --- | --- |
| Voice language selection | `PFD_Assignment/Views/Home/SelectLanguages.cshtml` |
| Favourite / recent shortcuts | `PFD_Assignment/DAL/FeatureDAL.cs`, `Views/Home/HomepageCashWithdraw.cshtml` |
| Face encoding and comparison | `PFD_Assignment/Controllers/HomeController.cs`, `Views/Home/FaceAuthentication.cshtml` |
| OTP interaction | `HomeController.cs`, `Views/Home/OtherOCBCAccount.cshtml` |
| Database structure | `database/schema.sql` |

Controller and view paths without a prefix are under `PFD_Assignment/`.

## Archive preparation in October 2026

This is a sanitised copy of the supplied assignment folder, not a claim that the original project was rebuilt after the assignment. The original local files are preserved.

- Removed embedded SMS credentials and phone configuration. SMS calls are disabled by default and require explicit environment configuration and opt-in.
- Removed personal face photographs, uploaded recordings, face-model weights, local IDE settings, compiled outputs and the old hosted chatbot widget.
- Removed automatic face-photo seeding on the language-selection route.
- Included database **structure only**, without customer, account, contact, PIN or face-image records.
- Replaced the connection string with a local example.
- Retained the original 180 translation resources. They were restored in a follow-up packaging commit; their content is unchanged.
- Omitted submission slides, briefs, reports, video and wireframes because they contain unrelated submission/personal material.
- Preserved supplied third-party source/license notices. The OCBC mark remains solely as part of the historical student prototype; it does not indicate endorsement.

Third-party license texts are under `docs/licenses/`: tracking.js versions 1.0.0/1.1.3 use the BSD license, and guessLanguage.js carries the LGPL 3.0-or-later notice reproduced in its source. The GNU GPL text accompanies the LGPL text. These files do not assign a new license to the team's assignment code or to the OCBC mark.

`docs/source-manifest.json` records original and archive source hashes. This publication cleanup is distinct from the original academic implementation.

## Build and runtime limits

The project targets .NET 6 and references FaceRecognitionDotNet, System.Data.SqlClient, Twilio and Google.Cloud.Speech.V1. The voice-selection implementation itself uses the browser Web Speech API; the presence of a Google Speech package does not mean it powers that feature.

```sh
dotnet build PFD_Assignment/PFD_Assignment.csproj
```

Building source is not a complete demo. End-to-end operation requires the matching ASP.NET runtime, a disposable SQL Server database with your own synthetic seed records, compatible native face-recognition dependencies and lawfully obtained model files. Those runtime dependencies and external services have not been exercised for this archive. Do not provide real customer data or use real financial accounts.

## Known limitations

This repository is published for learning and source review, not deployment:

- The peer-transfer handler does not enforce a server-side verified-OTP state before performing the transfer. The browser interaction is not a security boundary.
- OTP generation uses `Random`; there is no complete expiry, attempt-limit or session-binding design.
- PIN login and face matching are prototype mechanisms, with no liveness detection or measured error-rate evaluation.
- Transfer/session balance handling has correctness problems, including a repeated balance decrement. Banking-grade atomicity and authorisation are not established.
- The voice path is browser-specific and routes recognised speech between Chinese and English. It is not general multilingual voice control.
- The legacy framework and dependency versions need review and upgrading before any further application development.

No payments, SMS messages, face-camera captures or external AI-service calls were made during the portfolio review.
