# # Use .NET 9.0 SDK for the build stage
# FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build

# # WORKDIR /app
# # COPY *.csproj ./
# # RUN dotnet restore

# # COPY . .
# # RUN dotnet publish -c Release -o out

# # # Use .NET 9.0 runtime for the runtime stage
# # FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS runtime
# # WORKDIR /app
# # COPY --from=build /app/out .

# # ENTRYPOINT ["dotnet", "vidly.dll"]

# WORKDIR /App
# COPY vidly.sln ./
# COPY DotNet.Docker.csproj ./

# # Restore the solution
# RUN dotnet restore vidly.sln

# COPY . ./
# RUN dotnet publish vidly.sln -c Release -o out

# # Use .NET 9.0 runtime for the runtime stage
# FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS runtime
# WORKDIR /App
# COPY --from=build /App/out ./

# ENTRYPOINT ["dotnet", "vidly.dll"]


# Use .NET 9.0 SDK for the build stage
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build

WORKDIR /App

# Copy the solution and project file(s)
COPY vidly.sln ./
COPY DotNet.Docker.csproj ./  
# Ensure this points to the right .csproj file

# If your project is inside a folder like "src" or another folder, adjust the path here:
# COPY src/ ./src/

# Restore the solution
RUN dotnet restore vidly.sln

# Copy the rest of the source code
COPY . ./ 

# Publish the application
RUN dotnet publish vidly.sln -c Release -o out

# Use .NET 9.0 runtime for the runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS runtime
WORKDIR /App
COPY --from=build /App/out ./ 

ENTRYPOINT ["dotnet", "vidly.dll"]
