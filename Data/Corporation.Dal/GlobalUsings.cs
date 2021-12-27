// dotnet ef --startup-project ../../UI/Corporation.Web/ migrations add init --context CorporationContext

global using System;
global using System.Collections.Generic;
global using System.Linq;
global using System.Text;
global using System.Threading.Tasks;
global using Microsoft.EntityFrameworkCore;
global using Microsoft.Extensions.DependencyInjection;
global using Microsoft.Extensions.Logging;
global using Microsoft.Extensions.Configuration;
global using Microsoft.AspNetCore.Identity;
global using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

global using Corporation.Domain.Entites;
global using Corporation.Domain.Identity;
global using Corporation.Domain.Entites.Structures;