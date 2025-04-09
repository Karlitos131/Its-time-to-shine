using WebApplication1;
var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

var devices = new List<Device>
{
    new Device { Id = 1, Name = "Apple Watch", Type = "Watch" },
    new Device { Id = 2, Name = "LinuxPC", Type = "Linux" },
    new Device { Id = 3, Name = "IPhone", Type = "Phone" },
    new Device { Id = 4, Name = "ThinkPad", Type = "Tablet" },
};

app.MapGet("/devices", () =>
    devices.Select(d => new { d.Id, d.Name }));

app.MapGet("/devices/{id}", (int id) =>
{
    var device = devices.FirstOrDefault(d => d.Id == id);
    return device is not null ? Results.Ok(device) : Results.NotFound();
});

app.MapPost("/devices", (Device newDevice) =>
{
    newDevice.Id = devices.Count > 0 ? devices.Max(d => d.Id) + 1 : 1;
    devices.Add(newDevice);
    return Results.Created($"/devices/{newDevice.Id}", newDevice);
});

app.MapPut("/devices/{id}", (int id, Device updatedDevice) =>
{
    var device = devices.FirstOrDefault(d => d.Id == id);
    if (device is null) return Results.NotFound();

    device.Name = updatedDevice.Name;
    device.Type = updatedDevice.Type;
    return Results.Ok(device);
});

app.MapDelete("/devices/{id}", (int id) =>
{
    var device = devices.FirstOrDefault(d => d.Id == id);
    if (device is null) return Results.NotFound();

    devices.Remove(device);
    return Results.NoContent();
});

app.Run();
