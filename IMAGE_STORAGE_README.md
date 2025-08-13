# Image Storage System Implementation

## Overview
The system has been updated to store company logos in physical storage instead of the database. Images are now stored in the `wwwroot/images` directory and only the file path is stored in the database.

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

### 4. Enhanced Login Response
- **Company Information**: Includes company details and logo URL
- **Employee Information**: Includes employee details and company info
- **Logo URLs**: Full URLs for easy frontend integration

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

### Company Registration (Updated)
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
- Password: string (required)
- ConfirmPassword: string (required)
```

### Login Response (Enhanced)
```json
{
  "success": true,
  "message": "Login successful",
  "userId": "user-guid",
  "email": "company@example.com",
  "userName": "company@example.com",
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

### 2. Test Company Registration with Logo
```bash
curl -X POST "https://localhost:5001/api/auth/register/company" \
  -F "ArabicName=شركة تجريبية" \
  -F "EnglishName=Test Company" \
  -F "Email=company@test.com" \
  -F "PhoneNumber=+1234567890" \
  -F "WebsiteUrl=https://testcompany.com" \
  -F "LogoPath=@/path/to/your/logo.png" \
  -F "Password=TestPass123!" \
  -F "ConfirmPassword=TestPass123!"
```

### 3. Verify Image Storage
- Check `wwwroot/images/company-logos/` directory
- Verify image file exists with GUID filename
- Check database for `LogoPath` value

### 4. Test Login to Get Logo URL
```bash
curl -X POST "https://localhost:5001/api/auth/login" \
  -H "Content-Type: application/json" \
  -d '{
    "Email": "company@test.com",
    "Password": "TestPass123!"
  }'
```

### 5. Access Image via URL
- Use the `companyLogoUrl` from login response
- Access: `https://localhost:5001/images/company-logos/guid-filename.jpg`

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
