# Web applications

The purpose of this application is NOT define a standard but rather describe how we have been working so far and to give you a starting point for *microServiceBus.com*, *GreenEdge* and *SmartEdge*.

## General
All web applications are developed using normal ASP.Net MVC projects. framework. This being said, we rarely use it as a traditional MVC (More on that later).Each Controller have access to the EF context, ILogger and IConfiguration are always available through dependancy injection, along with other helpers and common resources.

Controller inherits from [Application].Controllers.BaseController, which in turn give access to user context and roles.

All projects follow normal C# naming convention except for Controller methods returning JsonResult (only called be JavaScript). These methods are named using camel case such as `saveWidget` or `getDashboard`.

All web applications are developed as **Single-page applications** (SPA), meaning the content (partial viiews) of all pages are loaded from start, while shown or hidden based on the menu selection. The exception of this *microServiceBus.com* which has one page per menu option, but otherwise adhere to the same concept.

### Disclaimer
Please note that while this application, code and standards is directly reflected in *GreenEdge* and *SmartEdge*, *microServiceBus.com* might differ slightly due to it's legacy.

### Scope
This sample only cover the Web application, not dependancies such as EF models or user models and federation etc.

### This sample
This Web application is managing incidents and aims to show how we work with lists, modals scripts and controllers.

### Partial views, Scripts, Styles and Bundles
All elements of the site are implemented as Partial views with a correlating JavaScript file and style sheet. For instance, the Administration page has five Partial views, each instantiated from the /Views/Admin/Index page. The contructor of this page also instantiate the associated JavaScript class and styles.

#### Controller
Controllers are used mainly for "API" calls from JavaScripts. These methods are named using *camelCase* mainly because it's easier to understand the context from which it's called but also because it's the naming convension of JS.

##### Constructors
In this sample we have a `IncidentsController`. The controller is initiated with a Db Context (fake) and a Logger. Constructor parameters are generally stored as *private readonly paramters*.

##### Methods
It has no methods returning `IActionResult` as it's only used form JS.

One of the methods is called `getIncidents` and return a `JsonResult`. These "API" methods never throws exceptions, but rather returns a dynamic object with an `error` field.

``` javascript
catch (Exception ex)
{
    return Json(new { error = "Something went wrong"});
}
```

Controllers should never return EF entities! EF entities should be mapped to a C# ViewModel, normally located in the Web application under `/Models/ViewModels/[Controller]/Models.cs` (we don't bother separating ViewModels into different files). Somtimes we don't even bother with a ViewModel at all and just return a `dynamic`.

#### JavaScript files
> Although there might be many normal JS files in the project, in this section we're focusing on the ones communicating with the controller.

Every controller has a correlating JS file located in `wwwroot/js/[Group]/[controller].js`. *Group* usually referes to a web sites over all navigation. For *GreenEdge* and *SmartEdge* it referes to **Home** or **Admin**. 

In this sample we have a JS file called `incidents.js`, which holds a class called `Incidents`. This file and class is instanciated in the `/Views/Home/Index` file.

##### The View Model...
Each JS file holds a knockout.js viewmodel. The viewmodel is always in sync with all elements of the partial view. HTML elements on the page is bound to a field (or array) in the viewModel. But before we get into the details of how the model is bound, lets have a look at how we create and populate the model...

The viewModel is a member of the JS class and is called `this.viewModel`. A viewModel can have two types of fields: `observable` or `observableArray`. In this sample we have *incidents* which is an array of Incidents and a *searchText* which is a **string**. View models may also have functions, but more on that later.

All fields of a view model are functions. To access a fields value you simply call the function:

```javascript
const searchText = this.viewModel.searchText();
```

To set the value, call the function with the parameter:

```javascript
this.viewModel.searchText("My new value");
```

Once you've created the viewModel, you need to apply it to an HTML element:

```javascript
ko.applyBindings(this.viewModel, document.getElementById("incidents-scope"));
```

This assumes there is an element on the Partial view called "incidents-scope", and the view model will ONLY apply to this scope. Should you need to access a view model from an other JS file, you can do so using the global variable (defined in Index.cshtml):

```javascript
const searchText = incidents.viewModel.searchText();
```

In this sample, we call the back-end controller to populate the viewModel:

```javascript
get(`/incidents/getIncidents?seachText=${this.viewModel.seachText()}`)
    .then(result => {
        if (result.error) {
            this.viewModel.errorText(result.error);
        }
        else {
            this.viewModel.incidents(result);
        }
    });
```

The `get` function is globaly defined in the `/js/site.js` file, together with a `post` function we'll be using later. The `get` function takes a *URI* parameter which in this case points to the `getIncident` method we looked at in the *Controller* section. The method return a JS promise which we usually handle with `.then`, but nothing is stopping you from using a async/await pattern if you prefer.

The return object is checked of `error`. If you recall from the *Controller*/*Method* section above. If the error field exist we populate the `errorText` field of the view model, while if everything is fine, we populate the `incidents` field:

#### Partial views
Partial views are instansiated from the `Index.cshtml` file, and always starts with a "scope"-div:
```html
<div id="incidents-scope" class="container-fluid">
```
The id of the div correlates to the `ko.applyBindings` call we made in the JS file.

https://knockoutjs.com/