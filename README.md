# Wisej.NET demo classes and application

Contains some of the sample code used to the create the videos on youtube.

**Disclaimer**
The code is provided as is with no guarantees and it is not be considered "best practice" or recommended procedure 
when developing Wisej.NET applications. Use this code on your own risk.

This code, however, might help if you want to get some insight in Wisej.NET programming and see how other developers 
approach certain aspects of programming business applications. Being a long time developer since the 80's it is, in 
my humble opinion, not always advisable to chase the latest en vogue trends of development paradigms and language 
features. Programming business applications requires business logic instead of syntax and maintainability, robustness 
and long-term availability are important topics.

**License**
The code provided in this repository is free to use as long as you don't sell it.

# WisejLib
*Author: Joe Meyer, ITG*

WisejLib is a library with support classes that I use in real world development based on Wisej.NET.
The library requires Dapper, Newtonsoft.Json and System.Data.Sqlite. I chose Dapper over Entity Framework 
because I'm an old school SQL developer and I find EF is overkill considering my actual needs. I also found 
it a bit too complicated, fragile and intransparent but that's just my personal opinion. So I chose Dapper 
and never regretted it. It's a very fast ORM, too.

The following classes are included:

## BindingList.cs

A Helper class that makes life easier with BindingSources in conjunction with DataGridViews and add/change/delete operations.
It contains methods for adding and deleting DbEntity objects. Deleted items go into the DeletedItems list and the class
provides a SaveChanges method that handles the entire database update process.

## CommandHandler.cs

An earlier Wisej.NET release introduced command processing already but this class was developed before that and hey, you never 
change a running system, do you?

My CommandHandler takes care of enabling/disabling buttons, menu items and component tools together with their click actions.
Create exactly 1 instance of CommandHandler on a form. For each Button, MenuItem and ComponentTool call one of the Register 
functions and pass delegates for enabling and executing. The rest is done by the CommandHandler.

Typical code might look like this:
```
    CommandHandler commands = new CommandHandler();
    commands
        .Register(btnOpen, null, () => Command_Open())
        .Register(btnProcess, () => !Busy, () => Command_Process())
        .Register(mnOpenMenuItem, btnOpen)
        ;
        
```
The above code defines 2 Buttons. btnOpen is always enabled because null is passed to the enableCallback paraneter. 
If btnOpen is clicked, the Command_Open() function is invoked.

The 2nd button is only enabled if the Busy property is false which means if you set Busy to false, btnProcess is 
automatically disabled. When clicked, Command_Process() is invoked.

The 3rd registration creates a command item for the mnOpenMenuItem menu item. Instead of assigning enabledCallback 
and executeCallback, btnOpen is passed. enabledCallback and executeCallback of btnOpen are copied to the 
mnOpenMenuItem menu item and it behaves exactly the same as the btnOpen button. This is useful when you have a 
context menu that duplicates the functionality of buttons on the form (for those old school users that prefer
context menus over textless buttons with cryptic icons ;-)

## DataBinder.cs

I dislike the BindingSource because I find it very tedious to select the bound field from the drop down dialog
each time I add an entry field. For lists of data in conjunction with a DataGridView it comes pretty handy but
for windows with many entry fields the BindingSource can become a pain to use.

A 2nd argument to not use BindingSource was that I was never able to make it work with DateTimePicker controls
when they are bound to a nullable DateTime property. The code that is generated in the forms deigner.cs file is
simply wrong.

This made me look into it and write my own binding class. On creation it scans all controls on a form and finds
properties of the model class that have a matching name. If so, it registers the control together with the associated 
property's PropertyInfo and attaches a Validating event handler to the control. Whenever the focus leaves the bound
control, the associated data class is updated as well

Because all model classes are derived from DbEntity and DbEntity implements INotifyPropertyChanged, the DataBinder
also attaches to the PropertyChanged event and gets notified when the data changes and it will update the control 
accordingly.

## DateUtils.cs

A helper class with extensions dealing with dates. It includes a couple of DateTime extensions for calculation 
the first and the last day of week/month/quarter/year. Another method returns date ranges based on an enum.

## DB.cs

DB is a small static class that handles connections to a Sqlite database. After you defined the connection 
string creating a connection with the DB class is as easy as this:
```
    using(var conn = DB.Connection)
    using(var tx = conn.BeginTransaction())
    {
        // your code goes here...
        tx.Commit();
    }
```

## DbAnnotations.cs

The DbAnnotations.cs file declares a few Attributes that are used on model classes to automatically generate SQL 
statements on the fly for inserting, updating and deleting

## DbEntity.cs

This is the mother of all model classes. It declares a primary key with the name RowId which must be present in every 
database table. RowId is the unique identifier of each database record in a table. You may disagree with me but hey,
I like it that way.

The DbEntity class is also able to inspect itself (and classes that are derived from DbEntity). It reads alle public
properties while taking DbAnnotation attributes into account and creates insert, update and delete statements for
interacting with the database. DbEntity has a SaveChanges method that makes use of these statements.

## DbSchema
The DbSchema class can read database tables and their fields from a Microsoft SqlServer database. This class is
not used in the demo application or in my videos, it's just here for the sake of completeness.

## DbSequence.cs

This is a helper class to deal with Microsoft SqlServer sequences. This class is not used in the demo application 
or in my videos, it's just here for the sake of completeness.

## Encryption.cs

This class is a wrapper to encrypting and decrypting strings using System.Security.Cryptography.
I know, it's not the safest cryptography thing in the world but hey, it's better than nothing.
Beneath encrypting and decrypting strings the class also provides a method to generate string passwords.

## Extension.cs

This static class provides a couple of extensions, mostly related to strings. Have a look at the code for details.
Some of the methods relate to German topics, like IsValidIBAN, IsValidPhoneNumber or ExpandGermanUmlauts. I am German, 
I live and work in Germany and I wrote applications for German companies, that’s why these methods are here. If you 
don’t need them you can simply ignore these.

## GridExtender.cs

I have made the experience that when dealing to DataGridViews I always write the same code over and over again, so 
I made the GridExtender. It provides extension methods to retrieving indexes and data of selected rows and even has
functions to load and restore the width, position and visibility of columns.

## HolidayTable.cs

I'm German and I write programs for German companies and in Germany we have official holidays that reoccur
every year. Unfortunately some of them are floating dates and, to make it worse, are not even valid in all German
states. This class calculates them all, a complete year with every calculation. There's also a cache component 
that can cache multiple years to avoid recalculating over and over again.

## Utils.cs

This class contains everything that I don't know where to put elsewhere. For example, it contains a method to create
a IBAN from account number and bank number (D/A/CH only). Or a few methods to display different extended MessageBoxes.
Or a method to clone objects. There will probably be more in the future, I guess.
The message box methods can parse the message and split it into dialog caption, title and actual message, all separated 
by the pipe symbol. It’s a flexible way of displaying message boxes with nice and structured text. And html tags are
supported, too.

## Validator.cs

A class that defines a couple of methods to check the input of some entry fields for validity. I found this helpful
but if you like other ways to validate input more... it's a free world, do as you like.

## WhereBuilder.cs

Another class that came from the fact that I was writing the same code over and over. The WhereBuilder eases the pain
of writing dynamic SQL WHERE clauses at runtime. It provides methods to add where conditions in a few different ways 
and at the end it provides a where clause combining all individual conditions. You can use it in your SQL statement then.
Since I'm using Dapper (and I love it!) this class helps me a lot. It also provides a method where you can pass 2 dates
and it will create a sql BETWEEN clause and also return the dynamic parameters needed for the execution of the Dapper query.

# DemoApp
*Author: Joe Meyer, ITG*

DemoApp is a Sqlite based application that I started to demonstrate how to make a Wisej.net application from scratch.
The way I chose is just one way to do it, you may find it better to follow a different approach, that's up to you.
However, by following how I extend this basic skeleton of an application you will get the idea and you will be able 
to write your own business application, even if you don’t like the way I did it.

## Visual Inheritance

**BaseForm** is derived from Wisej.Web.Form and all other form classes and forms are derived from BaseForm.
It contains a CommandHandler to deal with buttons and menu items.

**BaseDialog** is derived from BaseForm. It's FormBorderStyle is set to Wisej.Web.FormBorderStyle.Fixed and it is 
displayed centered to the parent form.

**BaseMdiChildForm** derives from BaseForm and is designed to be an mdi child of a Tabbed Mdi main form. It contains 
a dictionary for form parameters and a string to uniquely identify the form (see the comments of CreateMdiChildForm 
of the main form for details). A context menu is defined that appears on the tab of the form and it offers items to 
close mdi child forms. BaseMdiChildForms are created from the BaseMainForm by calling CreateMdiChildForm(...).

**BaseMainForm** derives from BaseForm and is the main form of an application. It contains a Wisej.Web.Ext.NavigationBar
for the user to access all the individual modules available in the application. CreateMdiChildForm is the most important
method, it create mdi child forms and passes the unique identifier and the dictionary of parameters to the mdi child.
BaseMdiForm also proovides AddNavItem which easily adds en entry in the navigation bar. See Forms/MainForm how the
navigation bar is populated and used.

## Classes

**Globals.cs** only holds a few fonts and the currently logged user. The user is stored in a session property because
global static properties are global to all clients of the application, not just for the current client. Therefore if
you want to keep a static property global to the current client only, you have to store it in a session variable.

**Initializer.cs** is a class that creates default data in the database which is required for the application to run.
For example, at least 1 superuser must be defined (with login name 'ADMIN'). It can also be used to initialize data
tables with test data.

**Lookup.cs** is a helper to populate combo boxes with lookup values. 

**LookupPair.cs** holds lookup data to be used in Lookup.cs. The class defines only the Id and Text property.

**Salutations.cs** defines the salutations (Mr., Mrs., Company) to be used with the Lookup class

**MainForm.cs** is the form that the application starts with. It already contains the navigation bar because it is inherited
from BaseMainForm. It contains just the methods LoginUser to authenticate the user, PopulateNavigationBar to fill the 
navigation bar and it adds the Load event handler to initialize a few things.

**LoginDialog.cs** is derived from BaseDialog. It contains a static Execute message for the ease of use. In fact all my
dialogs have a static Execute method that takes parameters, returns false when the user cancelled and that takes care
of creation and lifetime of the dialog. Look at the source, it is pretty much self explanatory.

## Schema

The Schema directory includes all model classes for database tables that are present in the database. Since this is 
just an early demo you won't find many classes here.

The classes all follow specific implementation guidelines by being derived from DbEntity (see WisejLib.DbEntity for 
details). The Schema classes do not implement any individual properties or methods, they simply reflect the underlying 
database table structure. If special properties or methods are needed, you would want to implement these in the according
Model class. This is designed this way to separate the ORM required classes from extending business logic. 
This repository also includes Schematix which is a program that reads the database schema from a Sqlite database and 
creates the Schema classes automatically. If I would implement anything else in a schema class, it would be overwritten
the next time I recreate the Schema class so instead of extending the schema class you better extend the model class that
is derived from the Schema class.

## Model

The Model directory includes 1 class per database table and it is derived from the according Schema class. For example,
DbUser is derived from Users, DbPermission is derived from Permissions. Opposite to the Schema classes the Model 
classes can be extended freely.
