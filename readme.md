Recently while trying to run IIS-SEO toolkit I discovered that it is not maintained anymore and it won't event install
in the recent version of the IIS. While searching the web I found a way to make this to work. Here is what you need to do:

1. Install the IIS SEO Toolkit from (https://www.microsoft.com/en-us/download/details.aspx?id=24823). Just download the 
IISSEO_amd64.msi file. 

2. Run regedit.exe on your machine and go to HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\InetStp, on the right side you
will find MajorVersion, copy the current value somewhere(this value depends on your version of IIS) change it to 7 for the time being

3. Now double click on the IISSEO_amd64.msi file and install it

4. Go back to regedit.exe on your machine and go to HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\InetStp and change the
value back to the old value

5. Go to IIS SEO Toolkit and now run your analysis on the website.

Chances are that your website has moved on from TLS 1.0, where microsoft toolkit uses tls 1.0. If this happens you will
get a violation error and it will exit. For workaround do the following...

1. In the IIS SEO Toolkit, on the right click on Edit Feature settings and set the directory to
C:\Users\{your user name}\Documents\IIS SEO Reports (means create a folder IIS SEO Reports under your documents directory)

1. Download and run this program with the site name as argument (e.g. seocrawler "https://www.officeclip.com") 
and wait for the program to finish.

2. Now refresh IIS and you will be able to view your report
