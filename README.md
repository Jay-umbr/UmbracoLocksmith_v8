# UmbracoLocksmith
Umbraco Backoffice Emergency Unlocker (v8)

## What does this do?
In case you have been locked out of your Umbraco 8 site backoffice/admin panel, and do not have an alternative login/anyone else on your team, your options are quite limited.

By default (most installations) Umbraco locks you out of the backoffice after 5 incorrect password attempts.

If you have no other means of getting access back, you can use this code to create a new admin user on website startup.

The credentials for the new admin user:

admin@admin.com

password123

## Before you try this approach

-Do you have anyone else on your team/in your company who can unlock your account?

-Is your project hosted on Umbraco Cloud? If so, you can reach out to Umbraco Support who can help.

-Are you able to configure SMTP settings in web.config to make sure reset password emails are being sent? (by default the SMTP will not be configured).


If you answered yes to any of the above, you do not need to use this code. Please consider the alternatives.

However, if, for some reason, you are unable to unlock your website access with any other method...

## How to use

Download the Locksmith.cs file and place it in App_Code folder in your website root. If you do not have an App_Code folder create one.
If you did everything right the App_Code should be next to App_Data and bin folders, on the same level.

Once done, restart the website. As in IIS/hosting restart. A user should be created once the frontpage fully loads - from there you can head on over to the backoffice and log in with the aforementioned credentials.

### Why App_Code?

Any .cs file put in App_Code folder will run on website startup. I chose this approach because it is the least invasive, implementation-wise - no routes etc are modified. 
The function to create a user simply runs on website startup.

Do note if you already have a user with email admin@admin.com the code will not do anything, and you might need to edit it a little bit.

### Requirements

All that is necessary to use this code is access to your website files.

# Warning

Immediately after logging in with the new user, change the username/password or deactivate that user (and make a new one instead).

Make sure to delete the file from App_Code as well.

I take no responsibility if this tool is used for nefarious purposes - it is simply meant to be a last resort option for people locked out of their Umbraco website.

## Final notes

-Works on any minor version 8 of Umbraco (tested 8.0, 8.1, 8.2, 8.3, 8.4, 8.5, 8.6, 8.7, 8.8, 8.9)

-the admin@admin.com user may seemingly replace the first user that existed in the backoffice (the user created during the Umbraco installation process). That user is not gone from the backoffice - they are simply invisible in the backoffice, and still exist in the database. As a result existing content/data is not affected.
