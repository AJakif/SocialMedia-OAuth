USE [master]
GO
/****** Object:  Database [Social]    Script Date: 5/11/2022 2:36:12 PM ******/
CREATE DATABASE [Social]
GO
ALTER DATABASE [Social] SET COMPATIBILITY_LEVEL = 150
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [Social].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [Social] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [Social] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [Social] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [Social] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [Social] SET ARITHABORT OFF 
GO
ALTER DATABASE [Social] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [Social] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [Social] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [Social] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [Social] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [Social] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [Social] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [Social] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [Social] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [Social] SET  DISABLE_BROKER 
GO
ALTER DATABASE [Social] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [Social] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [Social] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [Social] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [Social] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [Social] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [Social] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [Social] SET RECOVERY FULL 
GO
ALTER DATABASE [Social] SET  MULTI_USER 
GO
ALTER DATABASE [Social] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [Social] SET DB_CHAINING OFF 
GO
ALTER DATABASE [Social] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [Social] SET TARGET_RECOVERY_TIME = 60 SECONDS 
GO
ALTER DATABASE [Social] SET DELAYED_DURABILITY = DISABLED 
GO
ALTER DATABASE [Social] SET ACCELERATED_DATABASE_RECOVERY = OFF  
GO
EXEC sys.sp_db_vardecimal_storage_format N'Social', N'ON'
GO
ALTER DATABASE [Social] SET QUERY_STORE = OFF
GO
USE [Social]
GO
/****** Object:  Table [dbo].[Login]    Script Date: 5/11/2022 2:36:12 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Login](
	[OID] [varchar](32) NOT NULL,
	[id] [varchar](16) NULL,
	[password] [varchar](16) NULL,
	[loginType] [varchar](2) NULL,
	[status] [varchar](2) NOT NULL,
	[firstName] [varchar](128) NOT NULL,
	[lastName] [varchar](128) NOT NULL,
	[enrollmentDate] [datetime] NOT NULL,
	[email1] [varchar](64) NULL,
	[email2] [varchar](64) NULL,
	[securityQuestion] [varchar](256) NULL,
	[securityAnswer] [varchar](256) NULL,
	[googleId] [varchar](64) NULL,
	[facebookId] [varchar](64) NULL,
 CONSTRAINT [PK_Login] PRIMARY KEY CLUSTERED 
(
	[OID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
INSERT [dbo].[Login] ([OID], [id], [password], [loginType], [status], [firstName], [lastName], [enrollmentDate], [email1], [email2], [securityQuestion], [securityAnswer], [googleId], [facebookId]) VALUES (N'20220510153442-b23abeed62794dbc', NULL, NULL, NULL, N'A', N'Ahmed', N'Akif', CAST(N'2022-05-10T15:34:51.590' AS DateTime), N'aahmedjahin@gmail.com', NULL, NULL, NULL, N'101234237327514851020', N'4596027840502120')
INSERT [dbo].[Login] ([OID], [id], [password], [loginType], [status], [firstName], [lastName], [enrollmentDate], [email1], [email2], [securityQuestion], [securityAnswer], [googleId], [facebookId]) VALUES (N'20220510165701-5d988c6f29f94682', NULL, NULL, NULL, N'A', N'Ahmed Jahin', N'Akif', CAST(N'2022-05-10T16:57:01.830' AS DateTime), N'aahmedjahin.dev@gmail.com', NULL, NULL, NULL, N'111401545635434734690', NULL)
INSERT [dbo].[Login] ([OID], [id], [password], [loginType], [status], [firstName], [lastName], [enrollmentDate], [email1], [email2], [securityQuestion], [securityAnswer], [googleId], [facebookId]) VALUES (N'L00001', N'a.hasan', N'123', N'M', N'A', N'Asif', N'Hasan', CAST(N'2013-03-10T00:00:00.000' AS DateTime), N'asif@randomaccessinc.com', NULL, NULL, NULL, NULL, NULL)
INSERT [dbo].[Login] ([OID], [id], [password], [loginType], [status], [firstName], [lastName], [enrollmentDate], [email1], [email2], [securityQuestion], [securityAnswer], [googleId], [facebookId]) VALUES (N'L00002', N'm.mark', N'123', N'S', N'A', N'Mark', N'Millwood', CAST(N'2013-03-10T00:00:00.000' AS DateTime), N'mmark@randomaccessinc.com', NULL, NULL, NULL, NULL, NULL)
INSERT [dbo].[Login] ([OID], [id], [password], [loginType], [status], [firstName], [lastName], [enrollmentDate], [email1], [email2], [securityQuestion], [securityAnswer], [googleId], [facebookId]) VALUES (N'L00003', N'Akif', N'123', N'S', N'A', N'Ahmed Jahin', N'Akif', CAST(N'2022-05-09T16:45:20.167' AS DateTime), N'akifahmedjahin@gmail.com', NULL, NULL, NULL, N'104294054265391621837', NULL)
GO
USE [master]
GO
ALTER DATABASE [Social] SET  READ_WRITE 
GO
