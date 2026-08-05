# Lab 15 — StudentPortalWeb Answers

## Part B — Predict Before You Run

### B.1 — AddDbContext/Service Registration After `builder.Build()`

**Question:**  
If `builder.Services.AddControllersWithViews();` is added after `var app = builder.Build();`, will the app compile? Will the registration take effect?

**Answer:**  
The application will not work correctly because `builder.Build()` has already built the application's service provider. Service registrations should be completed before `builder.Build()`. Therefore, adding a service registration after `Build()` is not the correct stage for registering services, and the registration will not be part of the already-built application.

---

### B.2 — Unregistered Dependency

**Question:**  
A controller's constructor asks for a type that was never registered in `Program.cs`. When does the failure happen?

**Answer:**  
The failure happens when the application first tries to create that controller, which is normally when somebody visits a page that uses that controller. It is not a compile-time error because the compiler does not know whether a dependency has been registered in the DI container.

---

### B.3 — Middleware After Static Files

**Question:**  
If custom middleware is registered after `app.UseStaticFiles()` and the browser loads a page that links one CSS file, how many times will the middleware run?

**Prediction:**  
The prediction was that the middleware would not see requests handled before it by the Static Files middleware, so the static CSS request would not appear in its log.

**Actual result:**  
In this project, the application uses `app.MapStaticAssets()` rather than `app.UseStaticFiles()`. After moving the custom middleware in the available pipeline, the static resource requests were still logged. Therefore, the prediction was **incorrect for this project setup**.

---

# Part C — Wire the Real Context Through DI

### Screenshot — Initial Web Application

![Initial StudentPortalWeb page](screenshots/screenshot_1.png)

The screenshot above shows the initial MVC web application before the real student data was displayed.

The database context was registered through Dependency Injection before `builder.Build()`:

```csharp
builder.Services.AddDbContext<StudentPortalContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("StudentPortalContext")));
```

`StudentPortalContext` is injected into `HomeController`, and the `Index` action loads the students asynchronously, ordered by name.

The students are displayed in the view with:

- Full Name
- Year of Study
- GPA formatted to two decimal places

The page also displays the Build Number and the total number of students.

### Screenshot — Students Loaded From the Database

![Student Portal with database students](screenshots/screenshot_2.png)

The page shows the real students loaded from `StudentPortalContext`, with **4 students** displayed.

The count should match:

```sql
SELECT COUNT(*) FROM Students;
```

---

# Part D — Break It On Purpose

### D.2 — Remove the DbContext Registration

**Question:**  
What happens if `AddDbContext<StudentPortalContext>` is commented out?

**Answer:**  
The application can start, but the failure occurs when the HomeController needs to be created and its `StudentPortalContext` dependency cannot be resolved. The failure is therefore associated with the first request that needs the controller rather than a compile-time failure.

**Note:**  
The exact exception type and first sentence should be copied verbatim from the actual console/browser error if the lab requires exact wording.

---

### D.5 — Register DbContext as Singleton

The experiment used:

```csharp
//builder.Services.AddDbContext<StudentPortalContext>(options =>
//    options.UseSqlServer(builder.Configuration.GetConnectionString("StudentPortalContext")),
//    ServiceLifetime.Singleton);
```

**Observed result:**  
The application started and served the page normally.

**Answer:**  
This is bad news rather than good news. A `DbContext` is intended to be used for a limited lifetime and is not thread-safe; making it Singleton means the same context can be shared across multiple requests, which can eventually cause concurrency problems and unwanted shared state.

---

### D.6 — Restore the Default Lifetime

The context was restored to the default `Scoped` lifetime:

```csharp
builder.Services.AddDbContext<StudentPortalContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("StudentPortalContext")));
```

The page works again.

---

# Part E — The Lifetime Experiment

## Service Registration

The service was registered as **Scoped**:

```csharp
builder.Services.AddScoped<IYoussefStampService, YoussefStampService>();
```

A Scoped service gets one instance per HTTP request.

The service generates its Stamp in the constructor:

```csharp
Stamp = Guid.NewGuid().ToString().Substring(0, 8);
```

## E.1 — Did Stamp A and Stamp B match within a single load?

**Answer:**  
Yes. Stamp A and Stamp B matched within the same load because the service is registered as Scoped, so both injections use the same service instance within the same HTTP request.

### Screenshot — First Scoped Load

![First Scoped lifetime load](screenshots/screenshot_3.png)

This screenshot shows the first observed Scoped request, where Stamp A and Stamp B are the same.

## E.2 — Did the stamps change between loads?

**Answer:**  
Yes. The stamps changed between loads because each new HTTP request creates a new Scoped instance of the service, and the Stamp is generated in the constructor using `Guid.NewGuid()`.

### Screenshot — Second Scoped Load

![Second Scoped lifetime load](screenshots/screenshot_4.png)

This screenshot shows a later request where the Stamp values are different from the previous load, while Stamp A and Stamp B still match each other within that request.

## E.3 — Person Next to You

This answer depends on the actual lifetime and Lab ID assigned to the person next to you. It was not included in the provided material, so it should be filled in manually.

**Lab ID:** __________________

**Assigned Lifetime:** __________________

**Their observation:** __________________

---

# Part F — The Pipeline, Observed

## F.1–F.3 — Custom Middleware

The custom middleware was placed after `builder.Build()` and before the other middleware:

```csharp
app.Use(async (context, next) =>
{
    Console.WriteLine($"[START] {context.Request.Path}");

    if (context.Request.Path.Value?.Contains(ID.audit_path) == true)
    {
        Console.WriteLine(
            $"[AUDIT] Youssef Ezzat saw a request for {context.Request.Path}");
    }

    await next.Invoke();

    Console.WriteLine($"[END] {context.Request.Path}");
});
```

The Audit Path used during the recorded test was:

```text
/audit 34
```

which appeared in the encoded request path as:

```text
/audit%2034
```

---

## F.4 — Home Page Console Log

### Screenshot — Console Output

![Console output showing pipeline requests and Audit request](screenshots/screenshot_5.png)

The screenshot contains the recorded middleware output, including the home-page request, static-resource requests, and the Audit request.

### Console Log — Before Moving the Middleware

```text
info: Microsoft.Hosting.Lifetime[14]
      Now listening on: https://localhost:7032
info: Microsoft.Hosting.Lifetime[14]
      Now listening on: http://localhost:5126
info: Microsoft.Hosting.Lifetime[0]
      Application started. Press Ctrl+C to shut down.
info: Microsoft.Hosting.Lifetime[0]
      Hosting environment: Development
info: Microsoft.Hosting.Lifetime[0]
      Content root path: F:\iti\.NET Projects and Tasks\Session_15_ASP_NET_MVC_Overview_DI\Lab15_StudentPortalWeb\Lab15_StudentPortalWeb
[START] /
info: Microsoft.EntityFrameworkCore.Database.Command[20101]
      Executed DbCommand (19ms) [Parameters=[], CommandType='Text', CommandTimeout='30']
      SELECT [s].[Id], [s].[FullName], [s].[Gpa], [s].[YearOfStudy]
      FROM [Students] AS [s]
      ORDER BY [s].[FullName]
[END] /
[START] /lib/bootstrap/dist/css/bootstrap.min.css
[START] /Lab15_StudentPortalWeb.styles.css
[START] /css/site.css
[START] /lib/bootstrap/dist/js/bootstrap.bundle.min.js
[START] /lib/jquery/dist/jquery.min.js
[START] /js/site.js
[END] /js/site.js
[END] /css/site.css
[END] /Lab15_StudentPortalWeb.styles.css
[END] /lib/jquery/dist/jquery.min.js
[END] /lib/bootstrap/dist/js/bootstrap.bundle.min.js
[END] /lib/bootstrap/dist/css/bootstrap.min.css
```

### Answer

There were **7 `[START]` lines** for one home-page load.

The paths were:

1. `/`
2. `/lib/bootstrap/dist/css/bootstrap.min.css`
3. `/Lab15_StudentPortalWeb.styles.css`
4. `/css/site.css`
5. `/lib/bootstrap/dist/js/bootstrap.bundle.min.js`
6. `/lib/jquery/dist/jquery.min.js`
7. `/js/site.js`

**Why did one page load produce multiple requests?**

One page load produced multiple HTTP requests because the browser requests the main page and its referenced CSS and JavaScript resources separately.

---

# F.5 — Audit Path

### Console Log

```text
[START] /audit%2034
[AUDIT] Youssef Ezzat saw a request for /audit%2034
[END] /audit%2034
```

The URL returned a **404**, which was expected.

### Explanation

The middleware still ran because middleware processes the HTTP request before the request is matched to a controller. Therefore, a request can pass through the middleware even when no controller or endpoint exists for that URL.

---

# F.6–F.7 — Move the Middleware

### Console Log — After Moving the Middleware

```text
info: Microsoft.Hosting.Lifetime[14]
      Now listening on: https://localhost:7032
info: Microsoft.Hosting.Lifetime[14]
      Now listening on: http://localhost:5126
info: Microsoft.Hosting.Lifetime[0]
      Application started. Press Ctrl+C to shut down.
info: Microsoft.Hosting.Lifetime[0]
      Hosting environment: Development
info: Microsoft.AspNetCore.Hosting.Diagnostics[1]
      Request starting
[START] /
info: Microsoft.EntityFrameworkCore.Database.Command[20101]
      Executed DbCommand (22ms) [Parameters=[], CommandType='Text', CommandTimeout='30']
      SELECT [s].[Id], [s].[FullName], [s].[Gpa], [s].[YearOfStudy]
      FROM [Students] AS [s]
      ORDER BY [s].[FullName]
[END] /
[START] /css/site.css
[START] /lib/bootstrap/dist/css/bootstrap.min.css
[START] /Lab15_StudentPortalWeb.styles.css
[START] /lib/jquery/dist/jquery.min.js
[START] /lib/bootstrap/dist/js/bootstrap.bundle.min.js
[START] /js/site.js
[END] /js/site.js
[END] /css/site.css
[END] /Lab15_StudentPortalWeb.styles.css
[END] /lib/jquery/dist/jquery.min.js
[END] /lib/bootstrap/dist/js/bootstrap.bundle.min.js
[END] /lib/bootstrap/dist/css/bootstrap.min.css
```

### Answer

After moving the middleware in this project, **no paths disappeared** from the log.

The same home-page request and the CSS/JavaScript resource requests were still observed by the middleware.

**Why?**

The project uses:

```csharp
app.MapStaticAssets();
```

rather than:

```csharp
app.UseStaticFiles();
```

Therefore, the exact behavior described in the original `UseStaticFiles()` experiment does not map directly to this project's pipeline configuration.

### B.3 Comparison

**My B.3 prediction was incorrect for this project setup.**

I predicted that the static CSS request would disappear when the middleware was placed after Static Files, but the actual project uses `MapStaticAssets()`, and the static resource requests were still logged in the observed configuration.

---

# Final Pipeline State

After completing the experiment, the custom middleware was returned to the front of the pipeline, immediately after:

```csharp
var app = builder.Build();
```

This restored the original experiment configuration.

---

# Screenshot Reference

| Screenshot | Used in | What it shows |
|---|---|---|
| `screenshot_1.png` | Part C | Initial MVC web page |
| `screenshot_2.png` | Part C | Real students loaded from SQL Server |
| `screenshot_3.png` | Part E | First Scoped lifetime observation |
| `screenshot_4.png` | Part E | Second Scoped lifetime observation |
| `screenshot_5.png` | Part F | Console pipeline and Audit Path output |
