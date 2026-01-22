-- OES Database Creation -
CREATE DATABASE OnlineExamSystem;

USE OnlineExamSystem;

-- 1. User Table Creation --
CREATE TABLE Users (
    UserId INT IDENTITY(1,1) PRIMARY KEY,
    FullName NVARCHAR(100) NOT NULL,
    Email NVARCHAR(100) NOT NULL UNIQUE,
    PasswordHash NVARCHAR(256) NOT NULL,
    Mobile NVARCHAR(15) NULL,
    City NVARCHAR(50) NULL,
    State NVARCHAR(50) NULL,
    DOB DATE NULL,
    Qualification NVARCHAR(100) NULL,
    YearOfCompletion INT NULL,
    CreatedAt DATETIME DEFAULT GETDATE()
);

-- 2. Admin Table Creation --
CREATE TABLE Admin (
    AdminId INT IDENTITY(1,1) PRIMARY KEY,
    FullName NVARCHAR(100) NULL,
    Email NVARCHAR(100) NOT NULL UNIQUE,
    PasswordHash NVARCHAR(256) NOT NULL
);

-- 3. Levels Table Creation --
CREATE TABLE Levels (
    LevelId INT IDENTITY(1,1) PRIMARY KEY,
    LevelName NVARCHAR(50) NOT NULL UNIQUE,
    PassMarks INT NOT NULL
);

-- 4. Technologies Table Creation --
CREATE TABLE Technologies (
    TechId INT IDENTITY(1,1) PRIMARY KEY,
    Name NVARCHAR(50) NOT NULL UNIQUE,
    LevelId INT NOT NULL,
    CONSTRAINT FK_Technologies_Levels
        FOREIGN KEY (LevelId) REFERENCES Levels(LevelId)
);

-- 5. Questions Table Creation -- 
CREATE TABLE Questions (
    QuestionId INT IDENTITY(1,1) PRIMARY KEY,
    TechId INT NOT NULL,
    LevelId INT NOT NULL,
    QuestionText NVARCHAR(MAX) NOT NULL,
    CreatedAt DATETIME DEFAULT GETDATE(),
    CONSTRAINT FK_Questions_Technologies
        FOREIGN KEY (TechId) REFERENCES Technologies(TechId),
    CONSTRAINT FK_Questions_Levels
        FOREIGN KEY (LevelId) REFERENCES Levels(LevelId)
);

-- 6. Options Table Creation --
CREATE TABLE Options (
    OptionId INT IDENTITY(1,1) PRIMARY KEY,
    QuestionId INT NOT NULL,
    OptionText NVARCHAR(255) NOT NULL,
    IsCorrect BIT NOT NULL,
    CONSTRAINT FK_Options_Questions
        FOREIGN KEY (QuestionId) REFERENCES Questions(QuestionId)
);

-- 7. Exams Table Creation --
CREATE TABLE Exams (
    ExamId INT IDENTITY(1,1) PRIMARY KEY,
    UserId INT NOT NULL,
    TechId INT NOT NULL,
    LevelId INT NOT NULL,
    Score INT NOT NULL,
    Status BIT NOT NULL,
    StartedAt DATETIME NOT NULL,
    CompletedAt DATETIME NOT NULL,
    CONSTRAINT FK_Exams_Users
        FOREIGN KEY (UserId) REFERENCES Users(UserId),
    CONSTRAINT FK_Exams_Technologies
        FOREIGN KEY (TechId) REFERENCES Technologies(TechId),
    CONSTRAINT FK_Exams_Levels
        FOREIGN KEY (LevelId) REFERENCES Levels(LevelId)
);

-- 8. Results Table Creation --
CREATE TABLE Results (
    ResultId INT IDENTITY(1,1) PRIMARY KEY,
    ExamId INT NOT NULL,
    QuestionId INT NOT NULL,
    SelectedOptionId INT NOT NULL,
    IsCorrect BIT NOT NULL,
    CONSTRAINT FK_Results_Exams
        FOREIGN KEY (ExamId) REFERENCES Exams(ExamId),
    CONSTRAINT FK_Results_Questions
        FOREIGN KEY (QuestionId) REFERENCES Questions(QuestionId),
    CONSTRAINT FK_Results_Options
        FOREIGN KEY (SelectedOptionId) REFERENCES Options(OptionId)
);






