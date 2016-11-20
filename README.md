# Base64Image
---
an easy way to add images as a base64 encoded strings in your MVC Razor pages.

Examples:


Add the `Base64Image` package to your Web project
```sh
Install-Package Base64Image
```
Add a using to Base64Image namespace in your `.cshtml` view
```csharp
@using Base64Image
```

Add the following line to your `.cshtml` view, replace the base64 string with your image base64 string
```c
@Html.Base64ImageElement("iVBORw0KGgoAAAANSUhEUgAAAAUAAAAFCAYAAACNbyblAAAAHElEQVQI12P4//8/w38GIAXDIBKE0DHxgljNBAAO9TXL0Y4OHwAAAABJRU5ErkJggg == ")
```

---
To add an image from a file on your web server use the following example 
```c
@Html.Base64ImageFromFile("/Content/PIFN0.jpg")
```
replace `PIFn0.jpg` with your file relative path and name

---

For custom HTML attributes supply a dictionary with custom attirbutes
```c
@Html.Base64ImageFromFile("/Content/code-flat.png", new Dictionary<string, string> { { "class", "dottedBorder" }, { "id", "imageIdX" } })
```


Check out the Demo web project in my repository for more examples.


<span style="color:red; font-size:1em;">
You may need to add assembly binding redirects if you end up with conflicting MVC dll's versions.
</span>