FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build-env
WORKDIR /app

# copy csproj to workdir, restore nuggets
COPY *.csproj ./
RUN dotnet restore

# publish app to local dir
COPY . ./
RUN dotnet publish -c Release -o out

# run the runtime image, and copy over the built app
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build-env /app/out .

# RUN the app 
ENTRYPOINT [ "dotnet", "PlatformService.dll" ]


# To Run -> docker run -p 8080:8080 -d kylemorton5770/platformservice