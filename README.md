# AxiansSample.Web

This sample application is ment to provide a simple understanding of technologies commonly used for our applications.

The application in it self is very simple, it's just showing a list of incidents. All incidents are stored in a static context (no database), but is implemented to look like Entity Framework. 

> The point of this sample is to show how we use `knockout.js` and `SignalR`, along with the structure of our web applications.


## Project structure
Although we are using ASP.Net MVC, we want to provide a SPA experience. This means we seldom use *IActionResult* to pre-populate an MVC model, but rather call methods returning a *JsonResult* and use the result to populate a client side model.

Sections on the web page often corresponds to a menu item. such as *Emission sources* for *GreenEdge* or *Data sources* for *SmartEdge*.  Each section is implemented as a *Partial cshtml view* and  managed through it's own JavaScript class, which in turn communicates with a dedicated *Controller* API.

In this sample you have a Controller called **HomeController**, a partial view called **_Incidents.cshtml** and a JavaScript file called **incidents.js**. These are all the files you need to care about for now.

## Knockout.js
JavaScript classes, as described above, generally holds a (client) view-model which is implemented with Knockout.js. 

```javascript
class Incidents {
    constructor() {
        this.viewModel = {
            seachText: ko.observable("")
        };
```
Elements in the view model can then be referenced from HTML element in the partial view using the `data-bind` attribute.

```html
<input data-bind="value:seachText" />
```
> Note that we're binding  the searchText property of the view-model to the **value** of the element in this sample. However, you can bind data to any attribute of the element such as *color*, *visible*, *enabled* etc.

As Knockout.js provides a two-way binding, the value can be changed from either the element or from *javascript*, meaning if the user writes something in the input field, it automaticly updates the model. Likewise if the *seachText* value in the model changes, the input element will be updated instantly.

A typical scenario could begin with a call to the backend Controller to fetch some data, and then use that data to populate the view model. Assume the data is an array, we'd need a have a property of type `observableArray`:

```javascript
class Incidents {
    constructor() {
        this.viewModel = {
            seachText: ko.observable(""),
            incidents: ko.observableArray([]), // pre-set the value to an empty array
        };
```
With the view model updated, we can now call the back-end Controller and populate the model:

```javascript
get(`/incidents/getIncidents`)
    .then(result => {
        this.viewModel.incidents(result);
    });
```

With the model updated we can now bind the element using the `foreach` binding appied to the parent element:

```html
<div data-bind="foreach: products">
    <!-- everything here will be repeated for every item in the array-->
    <div class="card">
        <div class="card-body">
            <h5 class="card-title" data-bind="text:name"></h5>
            <p class="card-text" data-bind="text:description"></p>
            <button data-bind="click: $parent.addToCart">Add to cart</button>
        </div>
    </div>
</div>
```
> Please note that we're binding the name of each product to the `text` attribute as `h5` elements has no `value` attribute. Also worth noting is that every product will be displayed with a button, binded to view-model function called `addToCart`. The reference to the function need to be prefixed with `$parent` as it would otherwise referense the function as part of each product.

More details about knockout.js can be found at https://knockoutjs.com/ 


## SignalR
*SignalR* is an abstraction of WebSockets and allows us to call browser-clients from the back-end of our application. This might come  usefull when something has happend in a back-end service, unrelated to a client and you want to send a notification. The classic example is to monitor stock-exchange is real-time, which would otherwise have to be polling the back-end using some kind of interval.

### Enabling SignalR
SignalR is built in to ASP.Net so we don't need to add any *nuget* packages. However, we need to add the client side library and a SignalR hub. 

Follow this tutorial to add the library and the hub: [Add the SignalR client library](https://learn.microsoft.com/en-us/aspnet/core/tutorials/signalr?view=aspnetcore-8.0&tabs=visual-studio)


In this sample you incidents are loaded at startup using a call to the back-end similar to the sample in the *Knockour.js* section. However, you can generate more incidents by clicking the *Generate incidents* button. This action makes an async call to the `generateSomeIncidents` in the SignalR hub:

```javascript
// See incidents.js line 20
self.connection.invoke("generateSomeIncidents");
```
...this causes the SignalR hub to generate some incidents and call back to the (browser) client:

```csharp
// See IncidentHub.cs line 18
// Generate some new incidents....
...
// Get the ID of the calling client (browser)
var client = Clients.Client(this.Context.ConnectionId);
// Call updatedIncidents of the client
await client.SendAsync("updatedIncidents", incidents);
```
The client receives the call through this statement:
```javascript
// See incidents.js line 50 
this.connection.on("updatedIncidents", (incidents) =>{
    self.viewModel.incidents(incidents);
});
```
