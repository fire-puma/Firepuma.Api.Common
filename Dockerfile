FROM microsoft/dotnet:2.1-sdk AS builder
WORKDIR /src

ARG VERSION
RUN test -n "$VERSION" 
LABEL version=${VERSION:-development}

COPY . .
RUN dotnet restore
RUN dotnet build /p:Version=$VERSION -c Release --no-restore  
RUN dotnet pack /p:Version=$VERSION -c Release --no-restore --no-build --include-symbols -p:SymbolPackageFormat=snupkg -o /src/artifacts

ENTRYPOINT ["dotnet", "nuget", "push", "/src/artifacts/*.nupkg"]
