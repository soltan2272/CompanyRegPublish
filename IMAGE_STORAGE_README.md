# Image Storage System Implementation

## Overview
The system has been updated to store company logos in physical storage instead of the database. Images are now stored in the `wwwroot/images` directory and only the file path is stored in the database.

**NEW FLOW**: Users now set their password AFTER verifying OTP instead of during registration.

## Changes Made

### 1. Database Schema Update
- **Removed**: `LogoBytes` column (byte array)
- **Added**: `LogoPath` column (string) to store file paths
- **Migration**: `UpdateLogoStorage` migration created

### 2. New File Service
- **IFileService**: Interface for file operations
- **FileService**: Implementation for saving, deleting, and retrieving images
- **Features**:
  - Image validation (jpg, jpeg, png, gif, bmp)
  - Unique filename generation using GUID
  - Organized folder structure
  - File type validation

### 3. Updated Auth Service
- **Image Upload**: Now saves images to physical storage
- **Logo Path**: Stores relative path in database
- **Image URL**: Returns full URL for frontend consumption
- **New Flow**: Password set after OTP verification

### 4. Enhanced Login Response
- **Company Information**: Includes company details and logo URL
- **Employee Information**: Includes employee details and company info
- **Logo URLs**: Full URLs for easy frontend integration

## New User Registration Flow

### Step 1: User Registration (No Password)
```
POST /api/auth/register/company
Content-Type: multipart/form-data

Fields:
- ArabicName: string (required)
- EnglishName: string (required)
- Email: string (required)
- PhoneNumber: string (optional)
- WebsiteUrl: string (optional)
- LogoPath: file (optional)
```

**Response**: "Company registered successfully. Please check your email for verification."

### Step 2: OTP Generation
```
POST /api/auth/generate-otp
Content-Type: application/json

{
  "Email": "company@test.com"
}
```

**Response**: "OTP sent successfully"

### Step 3: OTP Verification
```
POST /api/auth/verify-otp
Content-Type: application/json

{
  "Email": "company@test.com",
  "Code": "123456"
}
```

**Response**: "Email verified successfully. Please set your password to complete registration."

### Step 4: Set Password
```
POST /api/auth/set-password
Content-Type: application/json

{
  "Email": "company@test.com",
  "Password": "MySecurePass123!",
  "ConfirmPassword": "MySecurePass123!"
}
```

**Response**: "Password set successfully. You can now login."

### Step 5: Login
```
POST /api/auth/login
Content-Type: application/json

{
  "Email": "company@test.com",
  "Password": "MySecurePass123!"
}
```

**Response**: Complete user information including logo URL

## Configuration

### appsettings.json
```json
{
  "FileStorage": {
    "UploadPath": "wwwroot/images",
    "BaseUrl": "/images"
  }
}
```

### Directory Structure
```
wwwroot/
└── images/
    └── company-logos/
        └── [uploaded logo files]
```

## API Endpoints

### Company Registration (Updated - No Password)
```
POST /api/auth/register/company
Content-Type: multipart/form-data
```

### Employee Registration (Updated - No Password)
```
POST /api/auth/register/employee
Content-Type: application/json
```

### OTP Generation
```
POST /api/auth/generate-otp
Content-Type: application/json
```

### OTP Verification
```
POST /api/auth/verify-otp
Content-Type: application/json
```

### Set Password After OTP (NEW)
```
POST /api/auth/set-password
Content-Type: application/json
```

### Login
```
POST /api/auth/login
Content-Type: application/json
```

## Login Response (Enhanced)
```json
{
  "success": true,
  "message": "Login successful",
  "userId": "user-guid",
  "email": "company@test.com",
  "userName": "company@test.com",
  "roles": ["Company"],
  "isEmailVerified": true,
  "companyName": "Test Company",
  "companyArabicName": "شركة تجريبية",
  "companyEnglishName": "Test Company",
  "companyWebsite": "https://testcompany.com",
  "companyLogoUrl": "/images/company-logos/guid-filename.jpg"
}
```

## Testing Instructions

### 1. Build and Run
```bash
cd CompanyApi
dotnet build
dotnet run
```

### 2. Test Company Registration (No Password)
```bash
curl -X POST "https://localhost:5001/api/auth/register/company" \
  -F "ArabicName=شركة تجريبية" \
  -F "EnglishName=Test Company" \
  -F "Email=company@test.com" \
  -F "PhoneNumber=+1234567890" \
  -F "WebsiteUrl=https://testcompany.com" \
  -F "LogoPath=@/path/to/your/logo.png"
```

### 3. Generate OTP
```bash
curl -X POST "https://localhost:5001/api/auth/generate-otp" \
  -H "Content-Type: application/json" \
  -d '{"Email": "company@test.com"}'
```

### 4. Verify OTP
```bash
curl -X POST "https://localhost:5001/api/auth/verify-otp" \
  -H "Content-Type: application/json" \
  -d '{"Email": "company@test.com", "Code": "123456"}'
```

### 5. Set Password
```bash
curl -X POST "https://localhost:5001/api/auth/set-password" \
  -H "Content-Type: application/json" \
  -d '{
    "Email": "company@test.com",
    "Password": "MySecurePass123!",
    "ConfirmPassword": "MySecurePass123!"
  }'
```

### 6. Login
```bash
curl -X POST "https://localhost:5001/api/auth/login" \
  -H "Content-Type: application/json" \
  -d '{
    "Email": "company@test.com",
    "Password": "MySecurePass123!"
  }'
```

## Security Benefits of New Flow

1. **Email Verification First**: Ensures user owns the email before setting password
2. **No Password in Registration**: Reduces risk of password exposure during registration
3. **Two-Factor Security**: OTP + Password provides better security
4. **Temporary Accounts**: Users can't login until they verify email and set password
5. **Password Reset Token**: Uses secure Identity password reset mechanism

## File Storage Features

### Security
- File type validation
- Unique filename generation
- Organized folder structure
- No direct file access to upload directory

### Performance
- Images served as static files
- No database bloat
- Faster image loading
- Reduced memory usage

### Scalability
- Easy to migrate to cloud storage (Azure Blob, AWS S3)
- Configurable storage paths
- Support for CDN integration

## Future Enhancements

### Cloud Storage Integration
```csharp
// Example Azure Blob Storage implementation
public class AzureBlobFileService : IFileService
{
    // Implementation for Azure Blob Storage
}
```

### Image Processing
- Automatic image resizing
- Thumbnail generation
- Image compression
- Format conversion

### CDN Integration
- CloudFront (AWS)
- Azure CDN
- Cloudflare
- Custom CDN solutions

## Troubleshooting

### Common Issues

1. **Images not accessible**
   - Verify `UseStaticFiles()` is configured in Program.cs
   - Check file permissions on wwwroot directory
   - Ensure image files exist in specified paths

2. **Upload failures**
   - Check file size limits
   - Verify file type is supported
   - Ensure upload directory has write permissions

3. **Database errors**
   - Run migrations: `dotnet ef database update`
   - Check connection string
   - Verify database schema

4. **Password setting failures**
   - Ensure OTP is verified first
   - Check password requirements (8+ chars, uppercase, lowercase, digit)
   - Verify user exists and email is verified

### Debug Information
- Check application logs for file service errors
- Verify file paths in database
- Test static file serving manually
- Check browser network tab for image requests

## Migration Notes

- **Existing Data**: Companies with LogoBytes will lose their logos
- **Backup**: Consider backing up existing logos before migration
- **Testing**: Test thoroughly in development before production deployment
- **Rollback**: Migration can be rolled back if needed
- **New Flow**: Users must follow the new OTP → Password flow
