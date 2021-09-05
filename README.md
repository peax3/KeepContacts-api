# KeepContacts
manage your contacts

## Features

- Perform CRUD (create, update, retrieve and delete) operations
- Authentication and Authorization of users
- Contact's Avatar upload with Cloudinary

### API Endpoints

```sh
# Register
https://{baseUrl}/accounts/register

# Login
https://{baseUrl}/accounts/login
```

The endpoints below require Authentication Bearer token - token is retrieved when a user registers or login successfully
```sh
# GET All Contacts
https://{baseUrl}/contacts

# GET Contact
https://{baseUrl}/contacts/{contactId}

# UPDATE Contact
https://{baseUrl}/contacts/{contactId}

# DELETE Contact
https://{baseUrl}/contacts/{contactId}

# UPLOAD Avatar 
https://{baseUrl}/contacts/{contactId}/avatar

# DELETE Avatar 
https://{baseUrl}/contacts/{contactId}/avatar/{avatarId}
```





