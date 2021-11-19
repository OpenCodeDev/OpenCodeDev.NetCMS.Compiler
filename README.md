# OpenCodeDev.NetCMS.Compiler
Official Compiler for NetCMS.

## Overview
This project is a compiler for NetCMS, we wanted to test Source Code Generator from Microsoft.

This project is quite messy, lots of testing around lots of antipatterns, we will probably clean it up someday but so far our focus is to harmonize the Compiler, Admin, Server and Shared Resources to allow Api Generator using full cutting edge technologies.

Everyone is welcome to contribute thru PRs


Open-Source .NET CMS Inspired of Hapi and Wordpress on many aspect and Strapi on Admin Side.

Goal is to offer an easy way to create api tables, execute CRUD and set permission.

Not only that but also allow to deploy api as cluster with central admin management so can be scaled lastly allow to sperate the admin dashboard from the api server, so can be hosted elsewhere.

Project still very early at idea stage we"ll build up from there using well mastered concepts.

## Why?
I never found exactly a good tool that is consistent especially when it come to data.

Example: Strapi sometime return {"key":null}, {"key":0} or {"key":{"id":0}}... data integrity is not there also if you build micro-services it can be complex to handle permissions and auth when contacting a specific service... especially when it comes to auth our goal here is to avoid having to contact other server for permission and auth purposes making request faster.

Because we want to leverage GRPC power but also allow classic JSON API wrapped around GRPC without having to create twice the behavior.

Lastly, while using strapi or wordpress it is great to have the administration built in but it can be a horrible thing when you have to handle multiple services... that why we aim to create the administration the same way strapi does with cutting edge Blazor tech-stack but can be easly hosted elsewhere like a seperated application but also can handle multiple related micro-service and integrate their data from a single dashboard.


# Compiler Parts
1. Create Project-> Pull starter project and create first required files.
2. Create Plugin -> Pull Plugin Starter Project and Create First Files.
3. Build -> Generate all C# common code based on model.json.
4. Roslyn -> Automatically add all generated code to project thru Roslyn C# 9.0

# NetCMS
- CMS is open and dynamic and can be changed on the fly.
- CMS uses JSON Model to Compile C# code.
- Any Changes to JSON Model requires a server restart.
- CMS Can make JSON Model via Admin Dashboard
- CMS Can set relationships within the same Opened CMS
- CMS Can set relationships from Opened CMS to Plugins: 1-0 and X-0 Only.
- CMS Can set Dominant Relationship (Delete Relation on Delete (1-0 Only)).
- CMS Supports Relationships: 0-1, 1-1, 1-0, X-0, 0-X, X-X, 1-X and X-1.
- CMS Uses EF Core (Code-First) and will bind relationship, X-0, 0-X, 1-0, 0-1 via a Binding Class.
  
# Plugins 
- Plugins are Sealed (User/Admin cannot change the api model of plugins its sealed)
- Each has a different DbContext to protect uniqueness and collisions between plugins.
- Plugins come in 4 Parts: Admin, App, Shared and Server.

# Ideas for Plugin Security
To activate plugin during development:
- NetCMS Project build must be set to debug.
- Plugin must be located in plugins folder at build location.

To activate plugin during release:
- Plugin must be signed for security (temper proof) and copyright (protect 3rd party).

Copyright protection (longterm):
- Publisher can allow to sell plugins only to OpenCodeDev Hosted CMS (No DLL PLugin Access).
- Publisher can use remote service to protect code and sell subscription.


Admin can force to allow custom unsigned plugin to run but notice will be visible on administration side.

KIC (Keep it Clean) Store Policy:

3rd parties can:
- Upload plugins to OCD NetCMS Pkgs Source
- To show plugin in our store front, plugin must be reviewed and tested (against redundant crap).
- Sell your plugins on our store for a fee unless paid in crypto (straight to your account).
- Sell Subscriptions thru our store.
Plugins cannot be selling subscriptions outside our store (user experience).
Plugins subscription can be in crypto or via credit card and you can setup webhook to get notify of any event regarding purchases with user informations.
- Upload Administrative Templates and Front-End Template
  
  One of the biggest advantage of NetCMS over other NodeJS or PHP CMS is the fact that front-end developpers will be able to develop design app and sell their template including the back-end which is quite great, rather than buy a template and having to shit yourself to make it work.

# Rough Goals
1. Avoid Using Reflection at Runtime (During Request), as much as possible.
2. Use Reflection at Compile Time, Make File Changes Before Compile.
3. Load Plugin at Runtime (Load Before server available) with Reflection.
4. Try to make a system that will avoid Hook thru reflection (maybe squash everything in during compiling?)

# Possible Roadmap
## 2021
   - Can Create GRPC/JSON Api routes and call its CRUD from GRPC and JSON POST/GET/PUT/DEL
   - Can Request 1 Depth Nested Relation to be return.
   - Can Fetch with Extensive Smart Search System.
   - Can Define and Set Public/Stric Private Model Fields.
   - Can Extend Administration Dashboard.
   - Support Plugin at Runtime.
   - Can Develop Back-End Plugins with Front-End (Dashboard).
   - Auto-Generate Basic Api Behavior (CRUD) with CLI.

## 2022
   - Completed Open-Source Client-Side.
   - Support for Deepnesting Level (Circular will be nulled).
   - Auto-Generated Basic Api Behavior (CRUD from Dashboard).
   - Fully Support Plugin at Runtime.
   - Thats all i could think of for the moment.

# NOTE TO MYSELF

Config -> Json Routes (Grpc or Traditional Api) Grpc:true, WebApi: true, DataContext: "Name" (Relation must be same DbContext)
Controllers -> Function are Called from (ControllerBase) by default.
Models -> Model Information (Table) Fields and Relations initially from (Model[Name]Base)
Services -> Set of Extra Functions Accessible from any api


App -> Create Api Route -> Construct
netcms generate:api -grpc -json "Recipes" -> Create Folder and Files (Json Config, C#)
netcms reconstruct -> Rebuild C# Classes of Model.
netcms remodel -> Update Message Request Fields
netcms refresh-legacy -> Check any change and Regenerate Wrapper for Grpc to JSON API
netcms prebuild "Solution Path"

netcms generate:api "Recipe"
	- Create Folder Named "Recipe" in "Server/Api"
	- Create Sub-Folders "Controllers", "Models", "Services"
	- Create Folder Named "Recipe" in "Shared/Api"
	- Create Sub-Folders "Controllers", "Models", "Services"

	- Create File "RecipeController.cs" in "Server/Api/Recipe/Controllers"
	- Create File "RecipeControllerLegacy.cs" in "Server/Api/Recipe/Controllers"
	- Create File "RecipePrivateModel.cs" in "Server/Api/Recipe/Models"
	- Create File "Recipeservice.cs" in "Server/Api/Recipe/Service"

	- Create File "IRecipeController.cs" in "Shared/Api/Controllers"
	- Create File "RecipeFetchRequest.cs" in "Shared/Api/Messages"
	- Create File "RecipePublicModel.cs" in "Shared/Api/Models"


Permission -> Handle Permission Logic and Behavior (Shared Across All Plugins)
  -> Load and List All OperationContract (Need to Test how to Detect Overrides and Hiarchi)
  -> Generates JWT Token, Validate and Decrypt Token
  -> Generates Api Keys, Validate and Decrypt
  -> As Master, When notified by account server that a role has changed on user, permission register user ID, Date of Change and Expiry.
  -> As Master, Whenever JWT is add or remove other slave-clusters are notified.
  -> As Slave, whenever i boot up, i notify the master and wait for greenlight.
  -> As Master, when slave notifies, master sends all active user changes, master lastly send green light signal. (Server up and runing)
  -> As Permission Plugin, when validating JWT's role, check if the requesting user's JWt creation date preceed the change date if so, send error to disconnect the user so it can get a new token. else allow acces since token date is later than last role change

  -> In-Memory Database 
  -> Which Role can call which Controller ? (CRUD)
  -> Which Role can See/Change which field (private = never show anyone like pwd) (restricted = specific role only) on request strip 

Permission.Account -> Handle Basic User Info Login (Register/Login/2FA/Reset PWD/Email Confirm)

Permission.ApiKey -> Handle ApiKeys (Create, Update, Delete)
