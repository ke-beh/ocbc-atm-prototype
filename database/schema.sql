-- Run only in an empty, disposable local database. No sample records are included.
CREATE TABLE dbo.Transaction_Type
(
    TransactionTypeID INT IDENTITY(1, 1) PRIMARY KEY,
    Name VARCHAR(50) NOT NULL
);
GO

CREATE TABLE dbo.Account_Type
(
    AccountTypeID INT IDENTITY(1, 1) PRIMARY KEY,
    Name VARCHAR(150) NOT NULL
);
GO

CREATE TABLE dbo.Account
(
    AccountNum VARCHAR(150) NOT NULL PRIMARY KEY,
    CardNumber VARCHAR(19) UNIQUE NULL,
    PIN VARBINARY(256) NOT NULL, -- It's advisable to store hashed PINs
    Biometrics VARBINARY(MAX) NULL, -- Placeholder for biometric data reference or encrypted data
    FaceImage VARBINARY(MAX) NULL, -- Placeholder for face image data reference or encrypted data
    DateOpened DATETIME NOT NULL,
    Amount MONEY NOT NULL,
    AccountStatus VARCHAR(25) DEFAULT 'Active', -- Assuming a new account is active by default
    AccountTypeID INT NOT NULL,
    FOREIGN KEY (AccountTypeID) REFERENCES dbo.Account_Type(AccountTypeID)
);
GO

CREATE TABLE dbo.[Transaction]
(
    TransactionID INT IDENTITY(1, 1) PRIMARY KEY,
    Description VARCHAR(100) NOT NULL,
    Amount MONEY NOT NULL,
    Balance MONEY NOT NULL, -- Assuming you want to record the balance after the transaction
    Location VARCHAR(100) NOT NULL,
    TransactionDate DATETIME NOT NULL,
    TransactionStatus VARCHAR(50) NOT NULL,
    AccountNum VARCHAR(150) NOT NULL,
    TransactionTypeID INT NOT NULL,
	FeedbackStarRating INT,
	FeedbackCategory VARCHAR (150) ,
	FeedbackString VARCHAR (150),
    FOREIGN KEY (TransactionTypeID) REFERENCES dbo.Transaction_Type(TransactionTypeID),
    FOREIGN KEY (AccountNum) REFERENCES dbo.Account(AccountNum)
);
GO


CREATE TABLE dbo.Customer
(
    CustomerID INT IDENTITY(1, 1) PRIMARY KEY,
    FirstName VARCHAR(50) NOT NULL,
    LastName VARCHAR(50) NOT NULL,
    Address VARCHAR(150) NULL,
    Phone VARCHAR(20) NULL,
    EmailAddress VARCHAR(100) NULL, -- Email addresses can be longer than 50 characters
    AccountNum VARCHAR(150) NOT NULL,
    FOREIGN KEY (AccountNum) REFERENCES dbo.Account(AccountNum)
);
GO

CREATE TABLE dbo.FeatureUsage
(
    FeatureUsageID INT IDENTITY(1,1) PRIMARY KEY,
    FeatureName VARCHAR(50) NOT NULL,
    UsageCount INT NOT NULL,
    CustomerID INT NOT NULL,
    LastUsed DATETIME NOT NULL DEFAULT GETDATE(), -- New column to track when the feature was last used
    FOREIGN KEY (CustomerID) REFERENCES dbo.Customer(CustomerID)
);
GO

CREATE TABLE dbo.OTPData
(
    OTPID INT IDENTITY(1,1) PRIMARY KEY,
    PhoneNumber VARCHAR(20) NOT NULL,
    OTPValue VARCHAR(6) NOT NULL,
    ExpirationTime DATETIME NOT NULL
);

-- Inserts to populate the reference tables


GO
