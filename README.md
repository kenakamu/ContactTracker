# Contact Tracker 

### Overview 
This is a Dynamics CRM Mobile SDK sample application. This sample application let you do followings.
- SignIn to your Dynamics CRM Online organizatoin.
- Search and display up tp 50 Contact records.
- Show the Contact detail.
- Create new Contact record. You can scan business card, take picture of the contact, or manually enter data.
- Edit existing Contact record.

This sample application uses following SDK.
- Mobile Development Helper Code for Dynamics CRM: <br/>
  https://code.msdn.microsoft.com/Mobile-Development-Helper-3213e2e6/
- OneNote API: <br/>
  http://dev.onenote.com/ <br/>
  https://msdn.microsoft.com/en-us/office/office365/howto/onenote-landing <br />
  https://channel9.msdn.com/Series/OneNoteDev

This sample application uses OAuth 2.0 and ADAL for authentication. See following link for more detail.

Dynamics CRM Developers: Build Your Own Mobile Apps for Windows, iOS, and Android<br/>
http://blogs.msdn.com/b/crminthefield/archive/2015/01/12/build-your-own-crm-mobile-app-s.aspx

### Prerequisites
To try the sample application, you need to have Microsoft Dynamics CRM Online and OneNote license. (trial license works.)
For CRM, you need to have create/read/update Contact privilege.

### How to try
You can download entire solution, compile and deploy to your phone with Visual Studio, or you can simply go to following link from your Windows Phone to download the app.

URL for Windows 10 https://www.microsoft.com/store/apps/9nblggh3spn6 <br/>
URL for Windows Phone 8.1 and earlier http://windowsphone.com/s?appid=9a43f50e-6293-43f9-930b-df1c416a7b3e 

##### Application flow
1. When you launch the application first time, you see SignIn page. Enter your Dynamics CRM Online URL and tap OK button in the bottom.
2. Once Signed In, you see existing Contact records. Try search contact.
3. By tapping a contact, detail page will be shown. You can tap emailaddress, phone number, or address to open related application.
4. By tapping Edit button, you can edit the contact.
5. By tapping Add button in main page, you can add new contact record.
6. By tapping Camera button in edit page, you can take a picture of the contact.
7. By tapping Scanner button in edit page, you can take a picture of business card, and it will be extracted automatically by using Office 365 OneNote API. Once the business card information is extracted, you can manully modify the data.
8. By tapping Save button in edit page, you can save the contact record.

##### About Business Card extraction
This sample application uses Office 365 OneNote API to extract business card.

1. Create OneNote page with default location of SignIn user, and send captured Business Card imgae.
2. OneNote API will generated VCF (Virtual Contact Card) as a result of extraction.
3. The sample application retrieves the created page and find VCF data link.
4. Once VCF downloaded, it deletes the OneNote Page.
5. Read VCF information and projection the result to Contact Edit page.
