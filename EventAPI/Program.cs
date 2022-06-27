using System.Net.Http.Headers;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddCors();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors(options => options.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
app.UseHttpsRedirection();

app.MapGet("/Events", () =>
{
    HttpClient client = new HttpClient();
    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/x-www-form-urlencoded"));

    HttpResponseMessage response = new();
    try {
        response = client.GetAsync("https://teg-coding-challenge.s3.ap-southeast-2.amazonaws.com/events/event-data.json").GetAwaiter().GetResult();
        response.EnsureSuccessStatusCode();

        if (response.StatusCode != System.Net.HttpStatusCode.OK && response.StatusCode != System.Net.HttpStatusCode.Created) {
            string errorMessage = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();
            throw new Exception(errorMessage);
        }

    }
    catch (System.Exception ex) {
        throw new Exception(ex.Message);
    }

    return response.Content.ReadAsStringAsync().GetAwaiter().GetResult();
});

app.Run();
