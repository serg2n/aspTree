FROM microsoft/dotnet:2.1-sdk-alpine AS build
WORKDIR /app

# layer and restore
COPY aspTree/*.sln ./aspTree/
COPY nuget.config . 
COPY aspTree/*.csproj ./aspTree/
RUN dotnet restore

# layer and build
COPY . .
WORKDIR /app/aspTree
RUN dotnet build

# layer adding linker then publish after tree shaking
FROM build AS publish
WORKDIR /app/aspTree
RUN dotnet add package ILLink.Tasks -v 0.1.5-preview-1841731 -s https://dotnet.myget.org/F/dotnet-core/api/v3/index.json
RUN dotnet publish -c Release -o out -r linux-musl-x64 /p:ShowLinkerSizeComparison=true 

# final layer using smallest runtime available
FROM microsoft/dotnet:2.1-runtime-deps-alpine AS runtime
ENV DOTNET_USE_POLLING_FILE_WATCHER=true
WORKDIR /app
COPY --from=publish /app/aspTree/out ./

# expose port and execute aspnetcore app
EXPOSE 80
ENTRYPOINT ["./aspTree"]