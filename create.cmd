dotnet new sln -n SpyStore
dotnet new classlib -n SpyStore.Dal -o .\SpyStore.Dal -f netcoreapp3.1
dotnet sln SpyStore.sln add SpyStore.Dal

dotnet new classlib -n SpyStore.Models -o .\SpyStore.Models -f netcoreapp3.1
dotnet sln SpyStore.sln add SpyStore.Models
dotnet new xunit -n SpyStore.Dal.Tests -o .\SpyStore.Dal.Tests -f netcoreapp3.1
dotnet sln SpyStore.sln add SpyStore.Dal.Tests
dotnet new xunit -n SpyStore.Service.Tests -o .\SpyStore.Service.Tests -f netcoreapp3.1
dotnet sln SpyStore.sln add SpyStore.Service.Tests
dotnet new webapi -n SpyStore.Service -au none --no-https -o .\SpyStore.Service -f netcoreapp3.1
dotnet sln SpyStore.sln add SpyStore.Service
dotnet new mvc -n SpyStore.Mvc -au none --no-https -o .\SpyStore.Mvc -f netcoreapp3.1
dotnet sln SpyStore.sln add SpyStore.Mvc
dotnet add SpyStore.Mvc reference SpyStore.Models
dotnet add SpyStore.Dal reference SpyStore.Models
dotnet add SpyStore.Dal.Tests reference SpyStore.Models
dotnet add SpyStore.Dal.Tests reference SpyStore.Dal
dotnet add SpyStore.Service reference SpyStore.Dal
dotnet add SpyStore.Service reference SpyStore.Models
dotnet add SpyStore.Service.Tests reference SpyStore.Models
dotnet add SpyStore.Service.Tests reference SpyStore.Dal