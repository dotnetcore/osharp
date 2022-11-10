global using System.ComponentModel;
global using System.ComponentModel.DataAnnotations;
global using System.ComponentModel.DataAnnotations.Schema;
global using System.Linq.Expressions;

global using Microsoft.Extensions.Caching.Distributed;
global using Microsoft.Extensions.DependencyInjection;
global using Microsoft.Extensions.Logging;

global using OSharp.Authorization.Dtos;
global using OSharp.Authorization.Entities;
global using OSharp.Authorization.EntityInfos;
global using OSharp.Authorization.Events;
global using OSharp.Caching;
global using OSharp.Collections;
global using OSharp.Core.Packs;
global using OSharp.Data;
global using OSharp.Dependency;
global using OSharp.Entity;
global using OSharp.EventBuses;
global using OSharp.Exceptions;
global using OSharp.Extensions;
global using OSharp.Filter;
global using OSharp.Identity.Entities;
global using OSharp.Mapping;
