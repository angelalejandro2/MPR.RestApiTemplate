from crewai import Tool
import os
import glob
import json
import re
from typing import List, Dict, Any

class CodeAnalyzerTool(Tool):
    name = "CodeAnalyzer"
    description = "Analyzes existing C# code to understand structure, patterns, and extension points"
    
    def _extract_namespaces(self, code: str) -> List[str]:
        namespace_pattern = r'namespace\s+([^\s{]+)'
        return re.findall(namespace_pattern, code)
    
    def _extract_classes(self, code: str) -> List[str]:
        class_pattern = r'class\s+([^\s:]+)'
        return re.findall(class_pattern, code)
    
    def _extract_methods(self, code: str) -> List[Dict[str, str]]:
        method_pattern = r'(public|private|protected|internal)\s+(async\s+)?([\w<>[\],\s]+)\s+(\w+)\s*\(([^)]*)\)'
        methods = []
        
        for match in re.finditer(method_pattern, code):
            visibility, is_async, return_type, name, parameters = match.groups()
            methods.append({
                "name": name,
                "visibility": visibility,
                "is_async": bool(is_async),
                "return_type": return_type.strip(),
                "parameters": parameters.strip()
            })
        
        return methods
    
    def _analyze_file(self, file_path: str) -> Dict[str, Any]:
        with open(file_path, 'r', encoding='utf-8') as f:
            content = f.read()
        
        return {
            "file_path": file_path,
            "namespaces": self._extract_namespaces(content),
            "classes": self._extract_classes(content),
            "methods": self._extract_methods(content)
        }
    
    def _run(self, project_path: str) -> str:
        csharp_files = glob.glob(os.path.join(project_path, "**/*.cs"), recursive=True)
        
        analysis_result = {
            "project_path": project_path,
            "file_count": len(csharp_files),
            "files": []
        }
        
        for file_path in csharp_files:
            analysis_result["files"].append(self._analyze_file(file_path))
        
        return json.dumps(analysis_result, indent=2)

class DatabaseSchemaAnalyzerTool(Tool):
    name = "DatabaseSchemaAnalyzer"
    description = "Analyzes existing database schema or EF Core models to understand data structure"
    
    def _extract_db_context(self, code: str) -> Dict[str, Any]:
        # Extract DbContext-derived class
        context_pattern = r'class\s+(\w+)\s*:\s*DbContext'
        context_matches = re.findall(context_pattern, code)
        
        if not context_matches:
            return {}
        
        context_name = context_matches[0]
        
        # Extract DbSet properties
        dbset_pattern = r'public\s+DbSet<(\w+)>\s+(\w+)\s*{\s*get;\s*set;\s*}'
        dbsets = []
        
        for match in re.finditer(dbset_pattern, code):
            entity_type, property_name = match.groups()
            dbsets.append({
                "entity_type": entity_type,
                "property_name": property_name
            })
        
        return {
            "context_name": context_name,
            "db_sets": dbsets
        }
    
    def _extract_entity_models(self, code: str) -> Dict[str, Any]:
        # Extract class name
        class_pattern = r'class\s+(\w+)'
        class_matches = re.findall(class_pattern, code)
        
        if not class_matches:
            return {}
        
        class_name = class_matches[0]
        
        # Extract properties
        property_pattern = r'public\s+([\w<>[\],\s]+)\s+(\w+)\s*{\s*get;\s*set;\s*}'
        properties = []
        
        for match in re.finditer(property_pattern, code):
            property_type, property_name = match.groups()
            properties.append({
                "type": property_type.strip(),
                "name": property_name
            })
        
        return {
            "class_name": class_name,
            "properties": properties
        }
    
    def _run(self, project_path: str, db_engine: str = None) -> str:
        csharp_files = glob.glob(os.path.join(project_path, "**/*.cs"), recursive=True)
        
        db_contexts = []
        entity_models = []
        
        for file_path in csharp_files:
            with open(file_path, 'r', encoding='utf-8') as f:
                content = f.read()
            
            if "DbContext" in content:
                context = self._extract_db_context(content)
                if context:
                    context["file_path"] = file_path
                    db_contexts.append(context)
            
            # Simple heuristic: files with properties but not DbContext might be entity models
            if "{ get; set; }" in content and "DbContext" not in content:
                model = self._extract_entity_models(content)
                if model:
                    model["file_path"] = file_path
                    entity_models.append(model)
        
        result = {
            "db_engine": db_engine,
            "db_contexts": db_contexts,
            "entity_models": entity_models
        }
        
        return json.dumps(result, indent=2)

class RequirementAnalyzerTool(Tool):
    name = "RequirementAnalyzer"
    description = "Analyzes business requirements and user stories to extract technical specifications"
    
    def _extract_entities(self, requirement: str) -> List[str]:
        # Simple noun phrase extraction (placeholder for NLP approach)
        common_entities = ["user", "customer", "order", "product", "account", "payment"]
        found_entities = []
        
        for entity in common_entities:
            if entity.lower() in requirement.lower():
                found_entities.append(entity)
        
        return found_entities
    
    def _extract_actions(self, requirement: str) -> List[str]:
        # Simple verb phrase extraction (placeholder for NLP approach)
        common_actions = ["create", "read", "update", "delete", "list", "search", "filter"]
        found_actions = []
        
        for action in common_actions:
            if action.lower() in requirement.lower():
                found_actions.append(action)
        
        return found_actions
    
    def _extract_api_endpoints(self, requirement: str, entities: List[str], actions: List[str]) -> List[Dict[str, str]]:
        endpoints = []
        
        for entity in entities:
            for action in actions:
                if action.lower() in requirement.lower() and entity.lower() in requirement.lower():
                    method = "GET"
                    if action.lower() in ["create"]:
                        method = "POST"
                    elif action.lower() in ["update"]:
                        method = "PUT"
                    elif action.lower() in ["delete"]:
                        method = "DELETE"
                    
                    endpoint = f"/api/{entity.lower()}s"
                    if action.lower() not in ["create", "list"]:
                        endpoint += "/{id}"
                    
                    endpoints.append({
                        "method": method,
                        "endpoint": endpoint,
                        "action": action,
                        "entity": entity
                    })
        
        return endpoints
    
    def _run(self, requirements: str) -> str:
        # Convert requirements to list if it's a string
        if isinstance(requirements, str):
            requirements_list = [requirements]
        else:
            requirements_list = requirements
        
        analysis_result = []
        
        for requirement in requirements_list:
            entities = self._extract_entities(requirement)
            actions = self._extract_actions(requirement)
            endpoints = self._extract_api_endpoints(requirement, entities, actions)
            
            analysis_result.append({
                "requirement": requirement,
                "entities": entities,
                "actions": actions,
                "api_endpoints": endpoints
            })
        
        return json.dumps(analysis_result, indent=2)

class CodeGeneratorTool(Tool):
    name = "CodeGenerator"
    description = "Generates C# code for API endpoints, business logic, and data access layer"
    
    def _generate_controller(self, entity_name: str, endpoints: List[Dict[str, str]]) -> str:
        controller_template = f"""
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using {entity_name}Service.Interfaces;
using {entity_name}Service.Models;

namespace API.Controllers
{{
    [ApiController]
    [Route("api/[controller]")]
    public class {entity_name}sController : ControllerBase
    {{
        private readonly I{entity_name}Service _{entity_name.lower()}Service;

        public {entity_name}sController(I{entity_name}Service {entity_name.lower()}Service)
        {{
            _{entity_name.lower()}Service = {entity_name.lower()}Service;
        }}

{self._generate_controller_methods(entity_name, endpoints)}
    }}
}}
"""
        return controller_template
    
    def _generate_controller_methods(self, entity_name: str, endpoints: List[Dict[str, str]]) -> str:
        methods = []
        
        for endpoint in endpoints:
            action = endpoint["action"]
            method = endpoint["method"]
            
            if action.lower() == "create":
                methods.append(f"""        [HttpPost]
        public async Task<ActionResult<{entity_name}Dto>> Create{entity_name}({entity_name}CreateDto {entity_name.lower()}Dto)
        {{
            var result = await _{entity_name.lower()}Service.Create{entity_name}({entity_name.lower()}Dto);
            return CreatedAtAction(nameof(Get{entity_name}), new {{ id = result.Id }}, result);
        }}""")
            elif action.lower() == "read" or action.lower() == "get":
                methods.append(f"""        [HttpGet("{{id}}")]
        public async Task<ActionResult<{entity_name}Dto>> Get{entity_name}(int id)
        {{
            var result = await _{entity_name.lower()}Service.Get{entity_name}(id);
            if (result == null)
                return NotFound();
                
            return Ok(result);
        }}""")
            elif action.lower() == "list":
                methods.append(f"""        [HttpGet]
        public async Task<ActionResult<List<{entity_name}Dto>>> GetAll{entity_name}s()
        {{
            var result = await _{entity_name.lower()}Service.GetAll{entity_name}s();
            return Ok(result);
        }}""")
            elif action.lower() == "update":
                methods.append(f"""        [HttpPut("{{id}}")]
        public async Task<ActionResult> Update{entity_name}(int id, {entity_name}UpdateDto {entity_name.lower()}Dto)
        {{
            if (id != {entity_name.lower()}Dto.Id)
                return BadRequest();
                
            var success = await _{entity_name.lower()}Service.Update{entity_name}({entity_name.lower()}Dto);
            if (!success)
                return NotFound();
                
            return NoContent();
        }}""")
            elif action.lower() == "delete":
                methods.append(f"""        [HttpDelete("{{id}}")]
        public async Task<ActionResult> Delete{entity_name}(int id)
        {{
            var success = await _{entity_name.lower()}Service.Delete{entity_name}(id);
            if (!success)
                return NotFound();
                
            return NoContent();
        }}""")
            elif action.lower() == "search" or action.lower() == "filter":
                methods.append(f"""        [HttpGet("search")]
        public async Task<ActionResult<List<{entity_name}Dto>>> Search{entity_name}s([FromQuery] {entity_name}SearchParams searchParams)
        {{
            var result = await _{entity_name.lower()}Service.Search{entity_name}s(searchParams);
            return Ok(result);
        }}""")
        
        return "\n\n".join(methods)
    
    def _generate_service_interface(self, entity_name: str, endpoints: List[Dict[str, str]]) -> str:
        interface_template = f"""
            using System.Collections.Generic;
            using System.Threading.Tasks;
            using {entity_name}Service.Models;

            namespace {entity_name}Service.Interfaces
            {{
                public interface I{entity_name}Service
                {{
            {self._generate_service_interface_methods(entity_name, endpoints)}
                }}
            }}
            """
        return interface_template
    
    def _generate_service_interface_methods(self, entity_name: str, endpoints: List[Dict[str, str]]) -> str:
        methods = []
        
        for endpoint in endpoints:
            action = endpoint["action"]
            
            if action.lower() == "create":
                methods.append(f"        Task<{entity_name}Dto> Create{entity_name}({entity_name}CreateDto {entity_name.lower()}Dto);")
            elif action.lower() == "read" or action.lower() == "get":
                methods.append(f"        Task<{entity_name}Dto> Get{entity_name}(int id);")
            elif action.lower() == "list":
                methods.append(f"        Task<List<{entity_name}Dto>> GetAll{entity_name}s();")
            elif action.lower() == "update":
                methods.append(f"        Task<bool> Update{entity_name}({entity_name}UpdateDto {entity_name.lower()}Dto);")
            elif action.lower() == "delete":
                methods.append(f"        Task<bool> Delete{entity_name}(int id);")
            elif action.lower() == "search" or action.lower() == "filter":
                methods.append(f"        Task<List<{entity_name}Dto>> Search{entity_name}s({entity_name}SearchParams searchParams);")
        
        return "\n".join(methods)
    
    def _generate_service_implementation(self, entity_name: str, endpoints: List[Dict[str, str]], db_engine: str) -> str:
        implementation_template = f"""
            using System.Collections.Generic;
            using System.Linq;
            using System.Threading.Tasks;
            using AutoMapper;
            using Microsoft.EntityFrameworkCore;
            using {entity_name}Service.Interfaces;
            using {entity_name}Service.Models;
            using {entity_name}Service.Data;

            namespace {entity_name}Service.Services
            {{
                public class {entity_name}Service : I{entity_name}Service
                {{
                    private readonly ApplicationDbContext _context;
                    private readonly IMapper _mapper;

                    public {entity_name}Service(ApplicationDbContext context, IMapper mapper)
                    {{
                        _context = context;
                        _mapper = mapper;
                    }}

            {self._generate_service_implementation_methods(entity_name, endpoints, db_engine)}
                }}
            }}
            """
        return implementation_template
    
    def _generate_service_implementation_methods(self, entity_name: str, endpoints: List[Dict[str, str]], db_engine: str) -> str:
        methods = []
        
        for endpoint in endpoints:
            action = endpoint["action"]
            
            if action.lower() == "create":
                methods.append(f"""        public async Task<{entity_name}Dto> Create{entity_name}({entity_name}CreateDto {entity_name.lower()}Dto)
                {{
                    var entity = _mapper.Map<{entity_name}>({entity_name.lower()}Dto);
                    _context.{entity_name}s.Add(entity);
                    await _context.SaveChangesAsync();
                    
                    return _mapper.Map<{entity_name}Dto>(entity);
                }}""")
            elif action.lower() == "read" or action.lower() == "get":
                methods.append(f"""        public async Task<{entity_name}Dto> Get{entity_name}(int id)
                {{
                    var entity = await _context.{entity_name}s
                        .AsNoTracking()
                        .FirstOrDefaultAsync(e => e.Id == id);
                        
                    return _mapper.Map<{entity_name}Dto>(entity);
                }}""")
            elif action.lower() == "list":
                methods.append(f"""        public async Task<List<{entity_name}Dto>> GetAll{entity_name}s()
                {{
                    var entities = await _context.{entity_name}s
                        .AsNoTracking()
                        .ToListAsync();
                        
                    return _mapper.Map<List<{entity_name}Dto>>(entities);
                }}""")
            elif action.lower() == "update":
                methods.append(f"""        public async Task<bool> Update{entity_name}({entity_name}UpdateDto {entity_name.lower()}Dto)
                {{
                    var entity = await _context.{entity_name}s.FindAsync({entity_name.lower()}Dto.Id);
                    if (entity == null)
                        return false;
                        
                    _mapper.Map({entity_name.lower()}Dto, entity);
                    await _context.SaveChangesAsync();
                    
                    return true;
                }}""")
            elif action.lower() == "delete":
                methods.append(f"""        public async Task<bool> Delete{entity_name}(int id)
                {{
                    var entity = await _context.{entity_name}s.FindAsync(id);
                    if (entity == null)
                        return false;
                        
                    _context.{entity_name}s.Remove(entity);
                    await _context.SaveChangesAsync();
                    
                    return true;
                }}""")
            elif action.lower() == "search" or action.lower() == "filter":
                search_method = f"""        public async Task<List<{entity_name}Dto>> Search{entity_name}s({entity_name}SearchParams searchParams)
                {{
                    var query = _context.{entity_name}s.AsQueryable();
                    
                    // Apply filters based on search parameters
                    if (!string.IsNullOrEmpty(searchParams.Keyword))
                    {{
                        query = query.Where(e => e.Name.Contains(searchParams.Keyword));
                    }}
                    
                    // Apply paging
                    if (searchParams.PageSize > 0)
                    {{
                        query = query.Skip((searchParams.PageNumber - 1) * searchParams.PageSize)
                                    .Take(searchParams.PageSize);
                    }}
                    
                    // Apply ordering
                    query = query.OrderBy(e => e.Name);
                    
                    var entities = await query.AsNoTracking().ToListAsync();
                    return _mapper.Map<List<{entity_name}Dto>>(entities);
                }}"""
                        
                # Add database-specific optimizations
                if db_engine.lower() == "sqlserver":
                    search_method = search_method.replace(
                        "var query = _context.{entity_name}s.AsQueryable();",
                        f"var query = _context.{entity_name}s.AsQueryable().TagWith(\"Search{entity_name}s\");"
                    )
                elif db_engine.lower() == "oracle":
                    search_method = search_method.replace(
                        "if (!string.IsNullOrEmpty(searchParams.Keyword))",
                        "// Oracle optimized case-insensitive search\nif (!string.IsNullOrEmpty(searchParams.Keyword))"
                    )
                
                methods.append(search_method)
        
        return "\n\n".join(methods)
    
    def _generate_entity_model(self, entity_name: str) -> str:
        model_template = f"""
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace {entity_name}Service.Models
{{
    public class {entity_name}
    {{
        public int Id {{ get; set; }}
        
        [Required]
        [MaxLength(100)]
        public string Name {{ get; set; }}
        
        public string Description {{ get; set; }}
        
        public DateTime CreatedAt {{ get; set; }} = DateTime.UtcNow;
        
        public DateTime? UpdatedAt {{ get; set; }}
        
        // Add navigation properties as needed
    }}
    
    public class {entity_name}Dto
    {{
        public int Id {{ get; set; }}
        public string Name {{ get; set; }}
        public string Description {{ get; set; }}
        public DateTime CreatedAt {{ get; set; }}
        public DateTime? UpdatedAt {{ get; set; }}
    }}
    
    public class {entity_name}CreateDto
    {{
        [Required]
        [MaxLength(100)]
        public string Name {{ get; set; }}
        
        public string Description {{ get; set; }}
    }}
    
    public class {entity_name}UpdateDto
    {{
        public int Id {{ get; set; }}
        
        [Required]
        [MaxLength(100)]
        public string Name {{ get; set; }}
        
        public string Description {{ get; set; }}
    }}
    
    public class {entity_name}SearchParams
    {{
        public string Keyword {{ get; set; }}
        public int PageNumber {{ get; set; }} = 1;
        public int PageSize {{ get; set; }} = 10;
    }}
}}
"""
        return model_template
    
    def _run(self, entity_name: str, endpoints: List[Dict[str, str]], db_engine: str = "sqlserver") -> str:
        generated_code = {
            "controller": self._generate_controller(entity_name, endpoints),
            "service_interface": self._generate_service_interface(entity_name, endpoints),
            "service_implementation": self._generate_service_implementation(entity_name, endpoints, db_engine),
            "entity_model": self._generate_entity_model(entity_name)
        }
        
        return json.dumps(generated_code, indent=2)

class PlaywrightTestGeneratorTool(Tool):
    name = "PlaywrightTestGenerator"
    description = "Generates Playwright automated tests for API endpoints"
    
    def _generate_test_fixture(self, entity_name: str) -> str:
        fixture_template = f"""
using System.Threading.Tasks;
using Microsoft.Playwright;
using Xunit;

namespace API.Tests.Fixtures
{{
    public class {entity_name}ApiFixture : IAsyncLifetime
    {{
        public IAPIRequestContext Request {{ get; private set; }}
        public string BaseUrl {{ get; private set; }} = "http://localhost:5000";
        
        public async Task InitializeAsync()
        {{
            var playwright = await Playwright.CreateAsync();
            Request = await playwright.APIRequest.NewContextAsync(new APIRequestNewContextOptions
            {{
                BaseURL = BaseUrl,
                IgnoreHTTPSErrors = true
            }});
        }}
        
        public async Task DisposeAsync()
        {{
            if (Request != null)
            {{
                await Request.DisposeAsync();
                Request = null;
            }}
        }}
    }}
}}
"""
        return fixture_template
    
    def _generate_api_test_class(self, entity_name: str, endpoints: List[Dict[str, str]]) -> str:
        test_class_template = f"""
using System.Threading.Tasks;
using System.Text.Json;
using Microsoft.Playwright;
using Xunit;
using API.Tests.Fixtures;
using System.Collections.Generic;
using System.Net;

namespace API.Tests
{{
    public class {entity_name}ApiTests : IClassFixture<{entity_name}ApiFixture>
    {{
        private readonly {entity_name}ApiFixture _fixture;
        private readonly string _apiEndpoint = "/api/{entity_name.lower()}s";
        
        public {entity_name}ApiTests({entity_name}ApiFixture fixture)
        {{
            _fixture = fixture;
        }}
        
{self._generate_api_test_methods(entity_name, endpoints)}
    }}
}}
"""
        return test_class_template
    
    def _generate_api_test_methods(self, entity_name: str, endpoints: List[Dict[str, str]]) -> str:
        methods = []
        
        for endpoint in endpoints:
            action = endpoint["action"]
            
            if action.lower() == "create":
                methods.append(f"""        [Fact]
        public async Task Create{entity_name}_ReturnsCreated_WithValidData()
        {{
            // Arrange
            var newItem = new
            {{
                name = "Test {entity_name}",
                description = "Test description"
            }};
            
            // Act
            var response = await _fixture.Request.PostAsync(_apiEndpoint, new APIRequestContextOptions
            {{
                DataObject = newItem
            }});
            
            // Assert
            Assert.Equal(HttpStatusCode.Created, (HttpStatusCode)response.Status);
            var responseBody = await response.JsonAsync();
            Assert.NotNull(responseBody);
            Assert.True(responseBody.Value.GetProperty("id").GetInt32() > 0);
            Assert.Equal(newItem.name, responseBody.Value.GetProperty("name").GetString());
        }}""")
            elif action.lower() == "read" or action.lower() == "get":
                methods.append(f"""        [Fact]
        public async Task Get{entity_name}_ReturnsItem_WhenItemExists()
        {{
            // Arrange - Create a test item first
            var newItem = new {{ name = "Test {entity_name}", description = "Test description" }};
            var createResponse = await _fixture.Request.PostAsync(_apiEndpoint, new APIRequestContextOptions
            {{
                DataObject = newItem
            }});
            var createResponseBody = await createResponse.JsonAsync();
            var id = createResponseBody.Value.GetProperty("id").GetInt32();
            
            // Act
            var response = await _fixture.Request.GetAsync($"{{_apiEndpoint}}/{{id}}");
            
            // Assert
            Assert.Equal(HttpStatusCode.OK, (HttpStatusCode)response.Status);
            var responseBody = await response.JsonAsync();
            Assert.NotNull(responseBody);
            Assert.Equal(id, responseBody.Value.GetProperty("id").GetInt32());
            Assert.Equal(newItem.name, responseBody.Value.GetProperty("name").GetString());
        }}
        
        [Fact]
        public async Task Get{entity_name}_ReturnsNotFound_WhenItemDoesNotExist()
        {{
            // Act
            var response = await _fixture.Request.GetAsync($"{{_apiEndpoint}}/99999");
            
            // Assert
            Assert.Equal(HttpStatusCode.NotFound, (HttpStatusCode)response.Status);
        }}""")
            elif action.lower() == "list":
                methods.append(f"""        [Fact]
        public async Task GetAll{entity_name}s_ReturnsList()
        {{
            // Act
            var response = await _fixture.Request.GetAsync(_apiEndpoint);
            
            // Assert
            Assert.Equal(HttpStatusCode.OK, (HttpStatusCode)response.Status);
            var responseBody = await response.JsonAsync();
            Assert.NotNull(responseBody);
            // Verify it's an array
            Assert.True(responseBody.Value.ValueKind == JsonValueKind.Array);
        }}""")
            elif action.lower() == "update":
                methods.append(f"""        [Fact]
        public async Task Update{entity_name}_ReturnsNoContent_WhenItemExists()
        {{
            // Arrange - Create a test item first
            var newItem = new {{ name = "Test {entity_name}", description = "Test description" }};
            var createResponse = await _fixture.Request.PostAsync(_apiEndpoint, new APIRequestContextOptions
            {{
                DataObject = newItem
            }});
            var createResponseBody = await createResponse.JsonAsync();
            var id = createResponseBody.Value.GetProperty("id").GetInt32();
            
            var updateItem = new
            {{
                id = id,
                name = "Updated {entity_name}",
                description = "Updated description"
            }};
            
            // Act
            var response = await _fixture.Request.PutAsync($"{{_apiEndpoint}}/{{id}}", new APIRequestContextOptions
            {{
                DataObject = updateItem
            }});
            
            // Assert
            Assert.Equal(HttpStatusCode.NoContent, (HttpStatusCode)response.Status);
            
            // Verify the update was successful
            var getResponse = await _fixture.Request.GetAsync($"{{_apiEndpoint}}/{{id}}");
            var getResponseBody = await getResponse.JsonAsync();
            Assert.Equal(updateItem.name, getResponseBody.Value.GetProperty("name").GetString());
        }}
        
        [Fact]
        public async Task Update{entity_name}_ReturnsBadRequest_WhenIdMismatch()
        {{
            // Arrange
            var updateItem = new
            {{
                id = 1,
                name = "Updated {entity_name}",
                description = "Updated description"
            }};
            
            // Act - Try to update with mismatched ID
            var response = await _fixture.Request.PutAsync($"{{_apiEndpoint}}/2", new APIRequestContextOptions
            {{
                DataObject = updateItem
            }});
            
            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, (HttpStatusCode)response.Status);
        }}""")
            elif action.lower() == "delete":
                methods.append(f"""        [Fact]
        public async Task Delete{entity_name}_ReturnsNoContent_WhenItemExists()
        {{
            // Arrange - Create a test item first
            var newItem = new {{ name = "Test {entity_name}", description = "Test description" }};
            var createResponse = await _fixture.Request.PostAsync(_apiEndpoint, new APIRequestContextOptions
            {{
                DataObject = newItem
            }});
            var createResponseBody = await createResponse.JsonAsync();
            var id = createResponseBody.Value.GetProperty("id").GetInt32();
            
            // Act
            var response = await _fixture.Request.DeleteAsync($"{{_apiEndpoint}}/{{id}}");
            
            // Assert
            Assert.Equal(HttpStatusCode.NoContent, (HttpStatusCode)response.Status);
            
            // Verify the item was deleted
            var getResponse = await _fixture.Request.GetAsync($"{{_apiEndpoint}}/{{id}}");
            Assert.Equal(HttpStatusCode.NotFound, (HttpStatusCode)getResponse.Status);
        }}
        
        [Fact]
        public async Task Delete{entity_name}_ReturnsNotFound_WhenItemDoesNotExist()
        {{
            // Act
            var response = await _fixture.Request.DeleteAsync($"{{_apiEndpoint}}/99999");
            
            // Assert
            Assert.Equal(HttpStatusCode.NotFound, (HttpStatusCode)response.Status);
        }}""")
            elif action.lower() == "search" or action.lower() == "filter":
                methods.append(f"""        [Fact]
        public async Task Search{entity_name}s_ReturnsFilteredList()
        {{
            // Arrange - Create test items with similar names
            var items = new[] 
            {{
                new {{ name = "Test {entity_name} ABC", description = "Test description 1" }},
                new {{ name = "Test {entity_name} XYZ", description = "Test description 2" }}
            }};
            
            foreach (var item in items)
            {{
                await _fixture.Request.PostAsync(_apiEndpoint, new APIRequestContextOptions
                {{
                    DataObject = item
                }});
            }}
            
            // Act - Search for specific keyword
            var response = await _fixture.Request.GetAsync($"{{_apiEndpoint}}/search?keyword=ABC");
            
            // Assert
            Assert.Equal(HttpStatusCode.OK, (HttpStatusCode)response.Status);
            var responseBody = await response.JsonAsync();
            Assert.NotNull(responseBody);
            Assert.True(responseBody.Value.ValueKind == JsonValueKind.Array);
            
            // Check that results contain only the items with the keyword
            var results = responseBody.Value.EnumerateArray();
            foreach (var result in results)
            {{
                Assert.Contains("ABC", result.GetProperty("name").GetString());
            }}
        }}""")
        
        return "\n\n".join(methods)
    
    def _run(self, entity_name: str, endpoints: List[Dict[str, str]]) -> str:
        generated_tests = {
            "test_fixture": self._generate_test_fixture(entity_name),
            "api_test_class": self._generate_api_test_class(entity_name, endpoints)
        }
        
        return json.dumps(generated_tests, indent=2)

class PerformanceAnalyzerTool(Tool):
    name = "PerformanceAnalyzer"
    description = "Analyzes and optimizes code for performance, with database-specific optimizations"
    
    def _analyze_query(self, query: str, db_engine: str) -> Dict[str, Any]:
        analysis = {
            "query": query,
            "db_engine": db_engine,
            "issues": [],
            "optimizations": []
        }
        
        # Check for common EF Core performance issues
        if "Include" in query and "ThenInclude" in query and ".ToList()" in query:
            analysis["issues"].append({
                "type": "eager_loading",
                "description": "Multiple eager loading operations may cause cartesian explosion",
                "severity": "high"
            })
            
            if db_engine.lower() == "sqlserver":
                analysis["optimizations"].append({
                    "description": "Use AsSplitQuery() to prevent cartesian explosion",
                    "code": query.replace(".Include", ".AsSplitQuery().Include")
                })
            elif db_engine.lower() == "oracle":
                analysis["optimizations"].append({
                    "description": "Use AsSplitQuery() to prevent cartesian explosion with Oracle",
                    "code": query.replace(".Include", ".AsSplitQuery().Include")
                })
        
        if ".Where" in query and not ".AsNoTracking()" in query and ".ToList()" in query:
            analysis["issues"].append({
                "type": "unnecessary_tracking",
                "description": "Query is tracking entities unnecessarily for read-only operation",
                "severity": "medium"
            })
            
            analysis["optimizations"].append({
                "description": "Use AsNoTracking() for read-only queries",
                "code": query.replace(".Where", ".AsNoTracking().Where")
            })
        
        if ".OrderBy" in query and ".Skip" in query and ".Take" in query:
            if db_engine.lower() == "sqlserver":
                analysis["optimizations"].append({
                    "description": "Add index to improve SQL Server paging performance",
                    "code": f"// Add index to the OrderBy column\n// CREATE NONCLUSTERED INDEX IX_OrderByColumn ON TableName(OrderByColumn);"
                })
            elif db_engine.lower() == "oracle":
                analysis["optimizations"].append({
                    "description": "Configure optimal fetch size for Oracle paging",
                    "code": "// In DbContext OnConfiguring:\noptions.UseOracle(connectionString, options => options.MaxBatchSize(100));"
                })
        
        # Database-specific optimizations
        if db_engine.lower() == "sqlserver":
            if not ".TagWith" in query and (".Where" in query or ".OrderBy" in query):
                analysis["optimizations"].append({
                    "description": "Tag queries for better profiling in SQL Server",
                    "code": query.replace("_context", "_context.TagWith(\"QueryName\")")
                })
        elif db_engine.lower() == "oracle":
            if ".Contains" in query:
                analysis["issues"].append({
                    "type": "inefficient_contains",
                    "description": "String Contains() may not use Oracle indexes efficiently",
                    "severity": "medium"
                })
                
                analysis["optimizations"].append({
                    "description": "Use EF.Functions.Like for better Oracle performance with indexing",
                    "code": query.replace(".Contains(", ".StartsWith(") + "\n// Or use: EF.Functions.Like(e.Property, $\"%{keyword}%\")"
                })
        
        return analysis
    
    def _analyze_db_context(self, context_code: str, db_engine: str) -> Dict[str, Any]:
        analysis = {
            "db_engine": db_engine,
            "issues": [],
            "optimizations": []
        }
        
        # Check for common DbContext configuration issues
        if not "optionsBuilder.UseLoggerFactory" in context_code:
            analysis["issues"].append({
                "type": "missing_logging",
                "description": "No query logging configured for performance troubleshooting",
                "severity": "low"
            })
            
            analysis["optimizations"].append({
                "description": "Add logger factory configuration for query logging",
                "code": """// In OnConfiguring:
optionsBuilder.UseLoggerFactory(LoggerFactory.Create(builder => builder.AddConsole()));"""
            })
        
        if not "optionsBuilder.EnableSensitiveDataLogging" in context_code and not "EnableSensitiveDataLogging" in context_code:
            analysis["optimizations"].append({
                "description": "Enable sensitive data logging for development environments",
                "code": """// In OnConfiguring for development only:
if (_environment.IsDevelopment())
{
    optionsBuilder.EnableSensitiveDataLogging();
}"""
            })
        
        # Database-specific optimizations
        if db_engine.lower() == "sqlserver":
            if not "optionsBuilder.UseSqlServer" in context_code or not "CommandTimeout" in context_code:
                analysis["optimizations"].append({
                    "description": "Configure SQL Server with appropriate command timeout",
                    "code": """optionsBuilder.UseSqlServer(
    connectionString,
    options => {
        options.CommandTimeout(30);
        options.EnableRetryOnFailure(3);
    });"""
                })
        elif db_engine.lower() == "oracle":
            if not "options.MaxBatchSize" in context_code:
                analysis["optimizations"].append({
                    "description": "Configure optimal batch size for Oracle",
                    "code": """optionsBuilder.UseOracle(
    connectionString,
    options => {
        options.MaxBatchSize(100);
        options.UseOracleSQLCompatibility("12");
    });"""
                })
        
        return analysis
    
    def _generate_optimization_recommendations(self, analysis: Dict[str, Any]) -> str:
        db_engine = analysis.get("db_engine", "sqlserver")
        recommendations = f"# Performance Optimization Recommendations for {db_engine.upper()}\n\n"
        
        if "issues" in analysis and analysis["issues"]:
            recommendations += "## Identified Issues\n\n"
            for issue in analysis["issues"]:
                recommendations += f"### {issue['type']} ({issue['severity']} severity)\n\n"
                recommendations += f"{issue['description']}\n\n"
        
        if "optimizations" in analysis and analysis["optimizations"]:
            recommendations += "## Recommended Optimizations\n\n"
            for i, opt in enumerate(analysis["optimizations"], 1):
                recommendations += f"### Optimization {i}: {opt['description']}\n\n"
                recommendations += f"```csharp\n{opt['code']}\n```\n\n"
        
        # Add database-specific general recommendations
        recommendations += "## General Recommendations\n\n"
        
        if db_engine.lower() == "sqlserver":
            recommendations += """- Use appropriate SQL Server indexing strategies for frequently queried columns
- Apply AsSplitQuery() for complex includes to prevent cartesian explosion
- Use compiled queries for frequently executed database operations
- Configure appropriate transaction isolation levels
- Apply strategic AsNoTracking() for read-only queries
- Consider SQL Server-specific features like table hints where appropriate
- Add query tags for easier profiling with SQL Server Profiler
- Configure command timeout settings appropriate for your operations
"""
        elif db_engine.lower() == "oracle":
            recommendations += """- Configure Oracle-specific connection settings in DbContext
- Use function-based indexes for complex filtering conditions
- Implement Oracle-specific batch processing for bulk operations
- Apply appropriate FetchSize configuration for large result sets
- Use EF.Functions.Like() instead of string Contains() for better index usage
- Use bind variables correctly to prevent hard parsing
- Configure statement caching appropriately
- Set appropriate MaxBatchSize for optimal performance
"""
        
        return recommendations
    
    def _run(self, code: str, db_engine: str = "sqlserver") -> str:
        if "DbContext" in code:
            analysis = self._analyze_db_context(code, db_engine)
        elif "Include" in code or "Where" in code or "OrderBy" in code:
            analysis = self._analyze_query(code, db_engine)
        else:
            analysis = {
                "db_engine": db_engine,
                "issues": [],
                "optimizations": []
            }
        
            recommendations = self._generate_optimization_recommendations(analysis)
            return recommendations
        # Check for cache expiration
        if not re.search(r'TimeSpan|ExpirationScanFrequency|AbsoluteExpiration|SlidingExpiration', code):
                analysis["issues"].append("Caching implemented but no explicit expiration policy detected")
                analysis["recommendations"].append("Set appropriate cache expiration policies to avoid stale data")
        return analysis
    
    def _analyze_validation(self, code: str) -> Dict[str, Any]:
        """Analyze validation implementation in the code"""
        analysis = {
            "validation_approach": None,
            "validation_coverage": 0,
            "issues": [],
            "recommendations": []
        }
        
        # Detect validation approach
        validation_approaches = {
            "Data Annotations": r'(\[Required\]|\[StringLength\]|\[Range\]|\[RegularExpression\])',
            "FluentValidation": r'(AbstractValidator|RuleFor|ValidationResult)',
            "Manual Validation": r'(if\s*\([^)]*==\s*null|if\s*\([^)]*\.Length|ModelState\.IsValid)',
            "Guard Clauses": r'(Guard\.|Throw\.|ArgumentNullException)'
        }
        
        for approach, pattern in validation_approaches.items():
            if re.search(pattern, code):
                analysis["validation_approach"] = approach
                break
        
        # Estimate validation coverage
        if analysis["validation_approach"]:
            # Count public methods with parameters
            public_methods = re.findall(r'public\s+(?:async\s+)?[\w<>[\],\s]+\s+\w+\s*\([^)]+\)', code)
            public_method_count = len(public_methods)
            
            # Count methods with validation
            validation_pattern = r'(ModelState\.IsValid|RuleFor|Required|StringLength|if\s*\([^)]*==\s*null)'
            methods_with_validation = re.findall(r'public\s+(?:async\s+)?[\w<>[\],\s]+\s+\w+\s*\([^)]+\)[^{]*\{[^}]*' + validation_pattern, code, re.DOTALL)
            methods_with_validation_count = len(methods_with_validation)
            
            # Calculate coverage percentage
            if public_method_count > 0:
                analysis["validation_coverage"] = round((methods_with_validation_count / public_method_count) * 100)
        
        # Identify issues and recommendations
        if not analysis["validation_approach"]:
            analysis["issues"].append("No structured validation approach detected")
            analysis["recommendations"].append("Implement a consistent validation approach (FluentValidation or Data Annotations recommended)")
        elif analysis["validation_coverage"] < 50:
            analysis["issues"].append(f"Low validation coverage ({analysis['validation_coverage']}%)")
            analysis["recommendations"].append("Increase validation coverage for all input parameters")
        
        if analysis["validation_approach"] == "Manual Validation":
            analysis["issues"].append("Using manual validation which can be inconsistent and error-prone")
            analysis["recommendations"].append("Consider using a structured validation approach like FluentValidation or Data Annotations")
        
        # Check for validation error handling
        if analysis["validation_approach"] and not re.search(r'(ModelState\.IsValid|ValidationResult|TryValidateModel)', code):
            analysis["issues"].append("Validation may be implemented but no proper error handling detected")
            analysis["recommendations"].append("Ensure validation errors are properly returned to clients")
        
        return analysis
    
    def _generate_cross_cutting_implementation(self, missing_concerns: List[str]) -> Dict[str, str]:
        """Generate implementation code for missing cross-cutting concerns"""
        implementations = {}
        
        for concern in missing_concerns:
            if concern == "logging":
                implementations["logging"] = self._generate_logging_implementation()
            elif concern == "error_handling":
                implementations["error_handling"] = self._generate_error_handling_implementation()
            elif concern == "caching":
                implementations["caching"] = self._generate_caching_implementation()
            elif concern == "validation":
                implementations["validation"] = self._generate_validation_implementation()
        
        return implementations
    
    def _generate_logging_implementation(self) -> str:
        """Generate logging implementation code"""
        return """
// 1. Add this to your .csproj file
// <ItemGroup>
//   <PackageReference Include="Microsoft.Extensions.Logging" Version="6.0.0" />
//   <PackageReference Include="Serilog.AspNetCore" Version="6.0.0" />
//   <PackageReference Include="Serilog.Sinks.Console" Version="4.1.0" />
//   <PackageReference Include="Serilog.Sinks.File" Version="5.0.0" />
// </ItemGroup>

// 2. Add this to Program.cs
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Configure Serilog
Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Information()
    .MinimumLevel.Override("Microsoft", Serilog.Events.LogEventLevel.Warning)
    .Enrich.FromLogContext()
    .WriteTo.Console()
    .WriteTo.File("logs/app-.log", rollingInterval: RollingInterval.Day)
    .CreateLogger();

builder.Host.UseSerilog();

// 3. Example of using logging in a controller
public class ExampleController : ControllerBase
{
    private readonly ILogger<ExampleController> _logger;

    public ExampleController(ILogger<ExampleController> logger)
    {
        _logger = logger;
    }

    [HttpGet]
    public IActionResult Get()
    {
        _logger.LogInformation("Getting resources");
        
        try
        {
            // Your code here
            return Ok();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while getting resources");
            throw;
        }
    }
}

// 4. Example of structured logging
public class StructuredLoggingExample
{
    private readonly ILogger<StructuredLoggingExample> _logger;

    public StructuredLoggingExample(ILogger<StructuredLoggingExample> logger)
    {
        _logger = logger;
    }

    public void ProcessOrder(Order order)
    {
        _logger.LogInformation("Processing order {OrderId} for customer {CustomerId} with total {Total}",
            order.Id, order.CustomerId, order.Total);
        
        // Instead of:
        // _logger.LogInformation($"Processing order {order.Id} for customer {order.CustomerId} with total {order.Total}");
    }
}
"""
    
    def _generate_error_handling_implementation(self) -> str:
        """Generate error handling implementation code"""
        return """
// 1. Create an error handling middleware
public class ErrorHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ErrorHandlingMiddleware> _logger;

    public ErrorHandlingMiddleware(RequestDelegate next, ILogger<ErrorHandlingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An unhandled exception occurred");
            await HandleExceptionAsync(context, ex);
        }
    }

    private static async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        context.Response.ContentType = "application/json";
        
        var response = new 
        {
            error = new 
            {
                message = "An error occurred while processing your request.",
                detail = exception.Message
            }
        };

        switch (exception)
        {
            case NotFoundException notFoundEx:
                context.Response.StatusCode = StatusCodes.Status404NotFound;
                response = new { error = new { message = notFoundEx.Message } };
                break;
                
            case ValidationException validationEx:
                context.Response.StatusCode = StatusCodes.Status400BadRequest;
                response = new { error = new { message = "Validation failed", errors = validationEx.Errors } };
                break;
                
            case UnauthorizedAccessException:
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                response = new { error = new { message = "Unauthorized access" } };
                break;
                
            default:
                context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                break;
        }

        await context.Response.WriteAsJsonAsync(response);
    }
}

// 2. Create custom exception classes
public class NotFoundException : Exception
{
    public NotFoundException(string message) : base(message) { }
}

public class ValidationException : Exception
{
    public IEnumerable<ValidationError> Errors { get; }

    public ValidationException(string message, IEnumerable<ValidationError> errors) : base(message)
    {
        Errors = errors;
    }
}

public class ValidationError
{
    public string PropertyName { get; set; }
    public string ErrorMessage { get; set; }
}

// 3. Register the middleware in Program.cs
var app = builder.Build();

// Add middleware (before routing middleware)
app.UseMiddleware<ErrorHandlingMiddleware>();

// 4. Example of throwing custom exceptions
public class ExampleService
{
    public async Task<Entity> GetByIdAsync(int id)
    {
        var entity = await _repository.GetByIdAsync(id);
        
        if (entity == null)
        {
            throw new NotFoundException($"Entity with ID {id} not found");
        }
        
        return entity;
    }
    
    public async Task CreateAsync(EntityDto dto)
    {
        // Validation example
        var validator = new EntityValidator();
        var validationResult = await validator.ValidateAsync(dto);
        
        if (!validationResult.IsValid)
        {
            var errors = validationResult.Errors.Select(e => new ValidationError 
            { 
                PropertyName = e.PropertyName, 
                ErrorMessage = e.ErrorMessage 
            });
            
            throw new ValidationException("Validation failed", errors);
        }
        
        // Processing logic
    }
}
"""
    
    def _generate_caching_implementation(self) -> str:
        """Generate caching implementation code"""
        return """
// 1. Add this to your .csproj file
// <ItemGroup>
//   <PackageReference Include="Microsoft.Extensions.Caching.Memory" Version="6.0.0" />
// </ItemGroup>

// 2. Register caching in Program.cs
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddMemoryCache();

// For distributed caching:
// builder.Services.AddDistributedMemoryCache();
// Or for Redis:
// builder.Services.AddStackExchangeRedisCache(options =>
// {
//     options.Configuration = builder.Configuration.GetConnectionString("Redis");
//     options.InstanceName = "Example";
// });

// 3. Create a caching service
public interface ICacheService
{
    T Get<T>(string key);
    void Set<T>(string key, T value, TimeSpan? absoluteExpiration = null);
    void Remove(string key);
}

public class MemoryCacheService : ICacheService
{
    private readonly IMemoryCache _cache;
    private readonly ILogger<MemoryCacheService> _logger;
    
    // Default expiration time of 15 minutes
    private readonly TimeSpan _defaultExpiration = TimeSpan.FromMinutes(15);

    public MemoryCacheService(IMemoryCache cache, ILogger<MemoryCacheService> logger)
    {
        _cache = cache;
        _logger = logger;
    }

    public T Get<T>(string key)
    {
        if (_cache.TryGetValue(key, out T cachedValue))
        {
            _logger.LogDebug("Cache hit for key: {Key}", key);
            return cachedValue;
        }
        
        _logger.LogDebug("Cache miss for key: {Key}", key);
        return default;
    }

    public void Set<T>(string key, T value, TimeSpan? absoluteExpiration = null)
    {
        var expirationTime = absoluteExpiration ?? _defaultExpiration;
        
        var cacheOptions = new MemoryCacheEntryOptions()
            .SetAbsoluteExpiration(expirationTime)
            .SetPriority(CacheItemPriority.Normal);
            
        _cache.Set(key, value, cacheOptions);
        
        _logger.LogDebug("Cache set for key: {Key} with expiration: {Expiration}", key, expirationTime);
    }

    public void Remove(string key)
    {
        _cache.Remove(key);
        _logger.LogDebug("Cache removed for key: {Key}", key);
    }
}

// 4. Register the cache service
builder.Services.AddSingleton<ICacheService, MemoryCacheService>();

// 5. Example of using caching in a service
public class CachedProductService
{
    private readonly IProductRepository _repository;
    private readonly ICacheService _cacheService;
    private readonly ILogger<CachedProductService> _logger;

    public CachedProductService(
        IProductRepository repository,
        ICacheService cacheService,
        ILogger<CachedProductService> logger)
    {
        _repository = repository;
        _cacheService = cacheService;
        _logger = logger;
    }

    public async Task<Product> GetByIdAsync(int id)
    {
        string cacheKey = $"product:{id}";
        
        // Try to get from cache first
        var cachedProduct = _cacheService.Get<Product>(cacheKey);
        
        if (cachedProduct != null)
        {
            return cachedProduct;
        }
        
        // Cache miss, get from repository
        var product = await _repository.GetByIdAsync(id);
        
        if (product != null)
        {
            // Cache the product with a 15-minute expiration
            _cacheService.Set(cacheKey, product, TimeSpan.FromMinutes(15));
        }
        
        return product;
    }

    public async Task<IEnumerable<Product>> GetAllAsync()
    {
        string cacheKey = "products:all";
        
        // Try to get from cache first
        var cachedProducts = _cacheService.Get<IEnumerable<Product>>(cacheKey);
        
        if (cachedProducts != null)
        {
            return cachedProducts;
        }
        
        // Cache miss, get from repository
        var products = await _repository.GetAllAsync();
        
        // Cache with a shorter expiration since the full list might change more frequently
        _cacheService.Set(cacheKey, products, TimeSpan.FromMinutes(5));
        
        return products;
    }

    public async Task UpdateAsync(Product product)
    {
        await _repository.UpdateAsync(product);
        
        // Invalidate cache
        _cacheService.Remove($"product:{product.Id}");
        _cacheService.Remove("products:all");
    }
}
"""
    
    def _generate_validation_implementation(self) -> str:
        """Generate validation implementation code"""
        return """
// 1. Add this to your .csproj file
// <ItemGroup>
//   <PackageReference Include="FluentValidation.AspNetCore" Version="11.0.0" />
// </ItemGroup>

// 2. Register FluentValidation in Program.cs
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers()
    .AddFluentValidation(fv => fv.RegisterValidatorsFromAssemblyContaining<Program>());

// 3. Create validator classes
public class ProductValidator : AbstractValidator<ProductDto>
{
    public ProductValidator()
    {
        RuleFor(p => p.Name)
            .NotEmpty().WithMessage("Name is required")
            .MaximumLength(100).WithMessage("Name cannot exceed 100 characters");
            
        RuleFor(p => p.Price)
            .GreaterThan(0).WithMessage("Price must be greater than zero");
            
        RuleFor(p => p.CategoryId)
            .NotEmpty().WithMessage("Category is required");
            
        // Conditional validation example
        When(p => p.IsOnSale, () => {
            RuleFor(p => p.SalePrice)
                .NotNull().WithMessage("Sale price is required when product is on sale")
                .GreaterThan(0).WithMessage("Sale price must be greater than zero")
                .LessThan(p => p.Price).WithMessage("Sale price must be less than regular price");
        });
    }
}

// 4. Example of a DTO with Data Annotations (alternative approach)
public class CustomerDto
{
    [Required(ErrorMessage = "Name is required")]
    [StringLength(100, ErrorMessage = "Name cannot exceed 100 characters")]
    public string Name { get; set; }
    
    [Required(ErrorMessage = "Email is required")]
    [EmailAddress(ErrorMessage = "Invalid email format")]
    public string Email { get; set; }
    
    [Phone(ErrorMessage = "Invalid phone number format")]
    public string Phone { get; set; }
    
    [Range(18, 120, ErrorMessage = "Age must be between 18 and 120")]
    public int Age { get; set; }
}

// 5. Example of validation in a controller using FluentValidation
[ApiController]
[Route("api/[controller]")]
public class ProductsController : ControllerBase
{
    private readonly IProductService _productService;
    
    public ProductsController(IProductService productService)
    {
        _productService = productService;
    }
    
    [HttpPost]
    public async Task<IActionResult> Create(ProductDto productDto)
    {
        // ModelState validation is handled automatically with [ApiController]
        // If validation fails, a 400 Bad Request is returned with validation errors
        
        var product = await _productService.CreateAsync(productDto);
        return CreatedAtAction(nameof(GetById), new { id = product.Id }, product);
    }
}

// 6. Example of manual validation in a service
public class OrderService
{
    private readonly IValidator<OrderDto> _validator;
    
    public OrderService(IValidator<OrderDto> validator)
    {
        _validator = validator;
    }
    
    public async Task<Order> CreateOrderAsync(OrderDto orderDto)
    {
        // Validate the order
        var validationResult = await _validator.ValidateAsync(orderDto);
        
        if (!validationResult.IsValid)
        {
            var errors = validationResult.Errors.Select(e => new ValidationError 
            { 
                PropertyName = e.PropertyName, 
                ErrorMessage = e.ErrorMessage 
            });
            
            throw new ValidationException("Order validation failed", errors);
        }
        
        // Process the valid order
        var order = new Order
        {
            // Map properties from DTO
        };
        
        // Save the order
        
        return order;
    }
}
"""
    
    def _run(self, code: str) -> str:
        """
        Analyze cross-cutting concerns implementation and suggest improvements
        """
        # Analyze various cross-cutting concerns
        logging_analysis = self._analyze_logging(code)
        error_handling_analysis = self._analyze_error_handling(code)
        caching_analysis = self._analyze_caching(code)
        validation_analysis = self._analyze_validation(code)
        
        # Identify missing or inadequate concerns
        missing_concerns = []
        
        if not logging_analysis["logging_framework_detected"]:
            missing_concerns.append("logging")
        
        if not error_handling_analysis["global_error_handling"]:
            missing_concerns.append("error_handling")
        
        if not caching_analysis["caching_detected"]:
            missing_concerns.append("caching")
        
        if not validation_analysis["validation_approach"]:
            missing_concerns.append("validation")
        
        # Generate implementation code for missing concerns
        implementations = self._generate_cross_cutting_implementation(missing_concerns)
        
        # Prepare comprehensive analysis report
        analysis_report = {
            "logging": logging_analysis,
            "error_handling": error_handling_analysis,
            "caching": caching_analysis,
            "validation": validation_analysis,
            "missing_concerns": missing_concerns,
            "implementations": implementations
        }
        
        return json.dumps(analysis_report, indent=2)

class DatabaseMigrationTool(Tool):
    name = "DatabaseMigrationTool"
    description = "Generates and manages database migration scripts for schema changes"
    
    def _extract_entity_models(self, code: str) -> List[Dict[str, Any]]:
        """Extract entity models from code"""
        entities = []
        
        # Look for class definitions that might be entity models
        class_pattern = r'public\s+class\s+(\w+)(?:\s*:\s*\w+)?\s*\{([^}]*)\}'
        class_matches = re.findall(class_pattern, code, re.DOTALL)
        
        for class_name, class_body in class_matches:
            # Skip obvious non-entity classes
            if class_name.endswith("Controller") or class_name.endswith("Service") or class_name.endswith("Factory"):
                continue
            
            # Look for properties that might indicate an entity
            property_pattern = r'public\s+([\w<>[\],\s]+)\s+(\w+)\s*{\s*get;\s*set;\s*}'
            properties = re.findall(property_pattern, class_body)
            
            if not properties:
                continue
            
            # Check if it has an Id property or key attribute
            has_id = any(prop_name == "Id" for _, prop_name in properties)
            has_key = "[Key]" in class_body
            
            if has_id or has_key:
                entity = {
                    "name": class_name,
                    "properties": []
                }
                
                # Process properties
                for prop_type, prop_name in properties:
                    property_info = {
                        "name": prop_name,
                        "type": prop_type.strip(),
                        "nullable": prop_type.strip().EndsWith("?"),
                        "attributes": []
                    }
                    
                    # Extract property attributes
                    attr_pattern = r'\[([^\]]+)\]\s*public\s+([\w<>[\],\s]+)\s+' + re.escape(prop_name)
                    attr_matches = re.findall(attr_pattern, class_body)
                    
                    if attr_matches:
                        for attr_match in attr_matches:
                            attribute = attr_match[0]
                            property_info["attributes"].append(attribute)
                    
                    entity["properties"].append(property_info)
                
                entities.append(entity)
        
        return entities
    
    def _extract_dbcontext(self, code: str) -> Dict[str, Any]:
        """Extract DbContext configuration from code"""
        context_info = {
            "name": None,
            "db_sets": [],
            "configurations": []
        }
        
        # Look for DbContext class
        context_pattern = r'public\s+class\s+(\w+)\s*:\s*DbContext\s*\{([^}]*)\}'
        context_matches = re.findall(context_pattern, code, re.DOTALL)
        
        if not context_matches:
            return context_info
        
        context_name, context_body = context_matches[0]
        context_info["name"] = context_name
        
        # Extract DbSet properties
        dbset_pattern = r'public\s+DbSet<(\w+)>\s+(\w+)\s*{\s*get;\s*set;\s*}'
        dbset_matches = re.findall(dbset_pattern, context_body)
        
        for entity_type, set_name in dbset_matches:
            context_info["db_sets"].append({
                "entity_type": entity_type,
                "set_name": set_name
            })
        
        # Look for entity configurations
        config_pattern = r'modelBuilder\.Entity<(\w+)>\(\)\s*\.((?:[^;}]|;(?=\s*\.))*)'
        config_matches = re.findall(config_pattern, code, re.DOTALL)
        
        for entity_type, config_body in config_matches:
            configuration = {
                "entity_type": entity_type,
                "configurations": []
            }
            
            # Extract individual configurations
            for config_line in config_body.strip().split('\n'):
                config_line = config_line.strip()
                if config_line and not config_line.startswith("//"):
                    configuration["configurations"].append(config_line)
            
            context_info["configurations"].append(configuration)
        
        return context_info
    
    def _generate_migration_script(self, new_entities: List[Dict[str, Any]], db_context: Dict[str, Any], db_engine: str) -> str:
        """Generate migration script for the detected entities"""
        migration_script = f"""
        // Entity Framework Core Migration Script
        // Generated for {db_engine}

        // 1. First, add Microsoft.EntityFrameworkCore.Tools package to your project:
        // <PackageReference Include="Microsoft.EntityFrameworkCore.Tools" Version="6.0.0" />

        // 2. Add the appropriate database provider:
        // For SQL Server:
        // <PackageReference Include="Microsoft.EntityFrameworkCore.SqlServer" Version="6.0.0" />
        // For Oracle:
        // <PackageReference Include="Oracle.EntityFrameworkCore" Version="6.0.0" />

        // 3. Execute the following commands in Package Manager Console:

        // To create a new migration:
        // Add-Migration InitialCreate -Context {db_context.get('name', 'ApplicationDbContext')}

        // To apply the migration to the database:
        // Update-Database -Context {db_context.get('name', 'ApplicationDbContext')}

        // 4. Alternatively, use the .NET CLI:
        // dotnet ef migrations add InitialCreate --context {db_context.get('name', 'ApplicationDbContext')}
        // dotnet ef database update --context {db_context.get('name', 'ApplicationDbContext')}

        // DbContext Example:
        public class {db_context.get('name', 'ApplicationDbContext')} : DbContext
        {
            public {db_context.get('name', 'ApplicationDbContext')}(DbContextOptions<{db_context.get('name', 'ApplicationDbContext')}> options)
                : base(options)
            {
            }

        """

        # Add DbSet properties
        for entity in new_entities:
            pass  # Ensure proper separation of statements
                migration_script += f"    public DbSet<{entity['name']}> {entity['name']}s {{ get; set; }}\n"
            }
                
            migration_script += "\n    protected override void OnModelCreating(ModelBuilder modelBuilder)\n    {\n"
            
            # Add entity configurations
            for entity in new_entities:
                migration_script += f"        modelBuilder.Entity<{entity['name']}>(entity =>\n        {{\n"
                
                id_property = next((p for p in entity["properties"] if p["name"] == "Id"), None)
                if id_property:
                    pass  # Ensure proper handling of the condition
                id_property = next((p for p in entity["properties"] if p["name"] == "Id"), None)
                if id_property:
                    migration_script += f"            entity.HasKey(e => e.Id);\n"
                
                # Add property configurations
                for prop in entity["properties"]:
                    if any("Required" in attr for attr in prop["attributes"]):
                        migration_script += f"            entity.Property(e => e.{prop['name']}).IsRequired();\n"
                    
                    if any("StringLength" in attr for attr in prop["attributes"]):
                        # Extract string length from attribute
                        string_length_attr = next((attr for attr in prop["attributes"] if "StringLength" in attr), None)
                        if string_length_attr:
                            length_match = re.search(r'StringLength\((\d+)\)', string_length_attr)
                            if length_match:
                                length = length_match.group(1)
                                migration_script += f"            entity.Property(e => e.{prop['name']}).HasMaxLength({length});\n"
                    
                    # Add database-specific configurations
                    if db_engine.lower() == "sqlserver":
                        if prop["type"] == "string" or prop["type"].startswith("string"):
                            migration_script += f"            entity.Property(e => e.{prop['name']}).HasColumnType(\"nvarchar(max)\");\n"
                        elif prop["type"] == "decimal" or prop["type"].startswith("decimal"):
                            migration_script += f"            entity.Property(e => e.{prop['name']}).HasColumnType(\"decimal(18,2)\");\n"
                    elif db_engine.lower() == "oracle":
                        if prop["type"] == "string" or prop["type"].startswith("string"):
                            migration_script += f"            entity.Property(e => e.{prop['name']}).HasColumnType(\"NVARCHAR2(2000)\");\n"
                        elif prop["type"] == "decimal" or prop["type"].startswith("decimal"):
                            migration_script += f"            entity.Property(e => e.{prop['name']}).HasColumnType(\"NUMBER(18,2)\");\n"
                
                migration_script += "        });\n\n"
            
            migration_script += "    }\n}\n"
            
            # Add migration notes
            migration_script += """
            // Important Notes:
            // 1. Always review generated migrations before applying them to production databases
            // 2. Consider creating seed data in migrations for reference or lookup tables
            // 3. For complex changes, consider using Fluent API in the DbContext.OnModelCreating method

            // Example of a data seeding operation:
            // modelBuilder.Entity<Category>().HasData(
            //     new Category { Id = 1, Name = "Electronics" },
            //     new Category { Id = 2, Name = "Books" }
            // );

            // Example of adding an index:
            // entity.HasIndex(e => e.Email).IsUnique();

            // Example of configuring a relationship:
            // modelBuilder.Entity<Order>()
            //     .HasOne(o => o.Customer)
            //     .WithMany(c => c.Orders)
            //     .HasForeignKey(o => o.CustomerId);
            """
        
        return migration_script
    
    def _generate_schema_change_script(self, old_entities: List[Dict[str, Any]], new_entities: List[Dict[str, Any]], db_engine: str) -> str:
        """Generate migration script for schema changes between old and new entities"""
        # Find entities that exist in both old and new
        common_entity_names = set(e["name"] for e in old_entities).intersection(set(e["name"] for e in new_entities))
        
        # Find new entities
        new_entity_names = set(e["name"] for e in new_entities) - set(e["name"] for e in old_entities)
        
        # Find removed entities
        removed_entity_names = set(e["name"] for e in old_entities) - set(e["name"] for e in new_entities)
        
        # Generate migration script
        migration_script = f"// Schema Change Migration Script for {db_engine}\n\n"
        
        # Add new tables
        if new_entity_names:
            migration_script += "// New Tables\n"
            for entity_name in new_entity_names:
                entity = next(e for e in new_entities if e["name"] == entity_name)
                
                if db_engine.lower() == "sqlserver":
                    migration_script += f"""
CREATE TABLE [dbo].[{entity_name}s] (
"""
                elif db_engine.lower() == "oracle":
                    migration_script += f"""
CREATE TABLE {entity_name.upper()}S (
"""
                
                # Add columns
                columns = []
                for prop in entity["properties"]:
                    column_def = ""
                    if db_engine.lower() == "sqlserver":
                        column_type = self._get_sql_server_type(prop["type"])
                        nullability = "NULL" if prop["nullable"] else "NOT NULL"
                        
                        if prop["name"] == "Id":
                            column_def = f"    [Id] {column_type} IDENTITY(1,1) PRIMARY KEY"
                        else:
                            column_def = f"    [{prop['name']}] {column_type} {nullability}"
                    elif db_engine.lower() == "oracle":
                        column_type = self._get_oracle_type(prop["type"])
                        nullability = "NULL" if prop["nullable"] else "NOT NULL"
                        
                        if prop["name"] == "Id":
                            column_def = f"    ID {column_type} PRIMARY KEY"
                        else:
                            column_def = f"    {prop['name'].upper()} {column_type} {nullability}"
                    
                    columns.append(column_def)
                
                migration_script += ",\n".join(columns)
                
                if db_engine.lower() == "sqlserver":
                    migration_script += "\n);\nGO\n\n"
                elif db_engine.lower() == "oracle":
                    migration_script += "\n);\n/\n\n"
        
        # Modified tables
        if common_entity_names:
            migration_script += "// Modified Tables\n"
            for entity_name in common_entity_names:
                old_entity = next(e for e in old_entities if e["name"] == entity_name)
                new_entity = next(e for e in new_entities if e["name"] == entity_name)
                
                old_props = {p["name"]: p for p in old_entity["properties"]}
                new_props = {p["name"]: p for p in new_entity["properties"]}
                
                # Find added properties
                added_props = set(new_props.keys()) - set(old_props.keys())
                if added_props:
                    migration_script += f"-- Add columns to {entity_name}s table\n"
                    for prop_name in added_props:
                        prop = new_props[prop_name]
                        
                        if db_engine.lower() == "sqlserver":
                            column_type = self._get_sql_server_type(prop["type"])
                            nullability = "NULL" if prop["nullable"] else "NOT NULL"
                            migration_script += f"ALTER TABLE [dbo].[{entity_name}s] ADD [{prop_name}] {column_type} {nullability};\nGO\n"
                        elif db_engine.lower() == "oracle":
                            column_type = self._get_oracle_type(prop["type"])
                            nullability = "NULL" if prop["nullable"] else "NOT NULL"
                            migration_script += f"ALTER TABLE {entity_name.upper()}S ADD {prop_name.upper()} {column_type} {nullability};\n/\n"
                
                # Find removed properties
                removed_props = set(old_props.keys()) - set(new_props.keys())
                if removed_props:
                    migration_script += f"-- Drop columns from {entity_name}s table\n"
                    for prop_name in removed_props:
                        if db_engine.lower() == "sqlserver":
                            migration_script += f"ALTER TABLE [dbo].[{entity_name}s] DROP COLUMN [{prop_name}];\nGO\n"
                        elif db_engine.lower() == "oracle":
                            migration_script += f"ALTER TABLE {entity_name.upper()}S DROP COLUMN {prop_name.upper()};\n/\n"
                
                # Find modified properties
                common_props = set(old_props.keys()).intersection(set(new_props.keys()))
                for prop_name in common_props:
                    old_prop = old_props[prop_name]
                    new_prop = new_props[prop_name]
                    
                    if old_prop["type"] != new_prop["type"] or old_prop["nullable"] != new_prop["nullable"]:
                        migration_script += f"-- Modify column in {entity_name}s table\n"
                        
                        if db_engine.lower() == "sqlserver":
                            column_type = self._get_sql_server_type(new_prop["type"])
                            nullability = "NULL" if new_prop["nullable"] else "NOT NULL"
                            migration_script += f"ALTER TABLE [dbo].[{entity_name}s] ALTER COLUMN [{prop_name}] {column_type} {nullability};\nGO\n"
                        elif db_engine.lower() == "oracle":
                            column_type = self._get_oracle_type(new_prop["type"])
                            nullability = "NULL" if new_prop["nullable"] else "NOT NULL"
                            migration_script += f"ALTER TABLE {entity_name.upper()}S MODIFY {prop_name.upper()} {column_type} {nullability};\n/\n"
        
        # Removed tables
        if removed_entity_names:
            migration_script += "// Removed Tables\n"
            for entity_name in removed_entity_names:
                if db_engine.lower() == "sqlserver":
                    migration_script += f"DROP TABLE [dbo].[{entity_name}s];\nGO\n\n"
                elif db_engine.lower() == "oracle":
                    migration_script += f"DROP TABLE {entity_name.upper()}S;\n/\n\n"
        
        return migration_script
    
    def _get_sql_server_type(self, type_name: str) -> str:
        """Convert C# type to SQL Server type"""
        type_mapping = {
            "string": "NVARCHAR(MAX)",
            "int": "INT",
            "long": "BIGINT",
            "bool": "BIT",
            "boolean": "BIT",
            "decimal": "DECIMAL(18,2)",
            "double": "FLOAT",
            "float": "REAL",
            "DateTime": "DATETIME2",
            "DateTimeOffset": "DATETIMEOFFSET",
            "Guid": "UNIQUEIDENTIFIER",
            "byte[]": "VARBINARY(MAX)"
        }
        
        # Remove nullable marker
        clean_type = type_name.replace("?", "").strip()
        
        # Check for collections
        if "List<" in clean_type or "ICollection<" in clean_type or "IEnumerable<" in clean_type:
            return "NVARCHAR(MAX)"  # Store as JSON
        
        return type_mapping.get(clean_type, "NVARCHAR(MAX)")
    
    def _get_oracle_type(self, type_name: str) -> str:
        """Convert C# type to Oracle type"""
        type_mapping = {
            "string": "NVARCHAR2(2000)",
            "int": "NUMBER(10)",
            "long": "NUMBER(19)",
            "bool": "NUMBER(1)",
            "boolean": "NUMBER(1)",
            "decimal": "NUMBER(18,2)",
            "double": "FLOAT",
            "float": "FLOAT(126)",
            "DateTime": "TIMESTAMP",
            "DateTimeOffset": "TIMESTAMP WITH TIME ZONE",
            "Guid": "RAW(16)",
            "byte[]": "BLOB"
        }
        
        # Remove nullable marker
        clean_type = type_name.replace("?", "").strip()
        
        # Check for collections
        if "List<" in clean_type or "ICollection<" in clean_type or "IEnumerable<" in clean_type:
            return "CLOB"  # Store as JSON
        
        return type_mapping.get(clean_type, "NVARCHAR2(2000)")
    
    def _generate_ef_core_migration_commands(self, context_name: str) -> str:
        """Generate EF Core commands for migration"""
        commands = f"""
// EF Core Migration Commands

// Using Package Manager Console:
Add-Migration InitialCreate -Context {context_name}
Update-Database -Context {context_name}

// Using .NET CLI:
dotnet ef migrations add InitialCreate --context {context_name}
dotnet ef database update --context {context_name}

// To generate an SQL script (for review or production deployment):
dotnet ef migrations script --context {context_name} --output migration.sql

// To remove last migration (if not applied to database):
dotnet ef migrations remove --context {context_name}

// To revert to a specific migration:
dotnet ef database update [MigrationName] --context {context_name}
"""
        return commands
    
    def _run(self, code: str, db_engine: str = "sqlserver", previous_version: str = None) -> str:
        """
        Generate database migration scripts for entity model changes
        """
        # Extract entity models from current code
        new_entities = self._extract_entity_models(code)
        
        # Extract DbContext information
        db_context = self._extract_dbcontext(code)
        
        # Generate migration script
        if previous_version:
            # Extract entity models from previous version
            old_entities = self._extract_entity_models(previous_version)
            
            # Generate schema change script
            migration_script = self._generate_schema_change_script(old_entities, new_entities, db_engine)
        else:
            # Generate initial migration script
            migration_script = self._generate_migration_script(new_entities, db_context, db_engine)
        
        # Generate EF Core commands
        ef_commands = self._generate_ef_core_migration_commands(db_context.get("name", "ApplicationDbContext"))
        
        # Prepare result
        result = {
            "entities": new_entities,
            "db_context": db_context,
            "migration_script": migration_script,
            "ef_core_commands": ef_commands
        }
        
        return json.dumps(result, indent=2)// Configure Serilog
Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Information()
    .MinimumLevel.Override("Microsoft", Serilog.Events.LogEventLevel.Warning)
    .Enrich.FromLogContext()
    .WriteTo.Console()
    .WriteTo.File("logs/app-.log", rollingInterval: RollingInterval.Day)
    .CreateLogger();

builder.Host.UseSerilog();

// 3. Example of using logging in a controller
public class ExampleController : ControllerBase
{
    private readonly ILogger<ExampleController> _logger;

    public ExampleController(ILogger<ExampleController> logger)
    {
        _logger = logger;
    }

    [HttpGet]
    public IActionResult Get()
    {
        _logger.LogInformation("Getting resources");
        
        try
        {
            // Your code here
            return Ok();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while getting resources");
            throw;
        }
    }
}

// 4. Example of structured logging
public class StructuredLoggingExample
{
    private readonly ILogger<StructuredLoggingExample> _logger;

    public StructuredLoggingExample(ILogger<StructuredLoggingExample> logger)
    {
        _logger = logger;
    }

    public void ProcessOrder(Order order)
    {
        _logger.LogInformation("Processing order {OrderId} for customer {CustomerId} with total {Total}",
            order.Id, order.CustomerId, order.Total);
        
        // Instead of:
        // _logger.LogInformation($"Processing order {order.Id} for customer {order.CustomerId} with total {order.Total}");
    }
}
"""
    
    def _generate_error_handling_implementation(self) -> str:
        """Generate error handling implementation code"""
        return """
// 1. Create an error handling middleware
public class ErrorHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ErrorHandlingMiddleware> _logger;

    public ErrorHandlingMiddleware(RequestDelegate next, ILogger<ErrorHandlingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An unhandled exception occurred");
            await HandleExceptionAsync(context, ex);
        }
    }

    private static async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        context.Response.ContentType = "application/json";
        
        var response = new 
        {
            error = new 
            {
                message = "An error occurred while processing your request.",
                detail = exception.Message
            }
        };

        switch (exception)
        {
            case NotFoundException notFoundEx:
                context.Response.StatusCode = StatusCodes.Status404NotFound;
                response = new { error = new { message = notFoundEx.Message } };
                break;
                
            case ValidationException validationEx:
                context.Response.StatusCode = StatusCodes.Status400BadRequest;
                response = new { error = new { message = "Validation failed", errors = validationEx.Errors } };
                break;
                
            case UnauthorizedAccessException:
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                response = new { error = new { message = "Unauthorized access" } };
                break;
                
            default:
                context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                break;
        }

        await context.Response.WriteAsJsonAsync(response);
    }
}

// 2. Create custom exception classes
public class NotFoundException : Exception
{
    public NotFoundException(string message) : base(message) { }
}

public class ValidationException : Exception
{
    public IEnumerable<ValidationError> Errors { get; }

    public ValidationException(string message, IEnumerable<ValidationError> errors) : base(message)
    {
        Errors = errors;
    }
}

public class ValidationError
{
    public string PropertyName { get; set; }
    public string ErrorMessage { get; set; }
}

// 3. Register the middleware in Program.cs
var app = builder.Build();

// Add middleware (before routing middleware)
app.UseMiddleware<ErrorHandlingMiddleware>();

// 4. Example of throwing custom exceptions
public class ExampleService
{
    public async Task<Entity> GetByIdAsync(int id)
    {
        var entity = await _repository.GetByIdAsync(id);
        
        if (entity == null)
        {
            throw new NotFoundException($"Entity with ID {id} not found");
        }
        
        return entity;
    }
    
    public async Task CreateAsync(EntityDto dto)
    {
        // Validation example
        var validator = new EntityValidator();
        var validationResult = await validator.ValidateAsync(dto);
        
        if (!validationResult.IsValid)
        {
            var errors = validationResult.Errors.Select(e => new ValidationError 
            { 
                PropertyName = e.PropertyName, 
                ErrorMessage = e.ErrorMessage 
            });
            
            throw new ValidationException("Validation failed", errors);
        }
        
        // Processing logic
    }
}
"""
    
    def _generate_caching_implementation(self) -> str:
        """Generate caching implementation code"""
        return """
// 1. Add this to your .csproj file
// <ItemGroup>
//   <PackageReference Include="Microsoft.Extensions.Caching.Memory" Version="6.0.0" />
// </ItemGroup>

// 2. Register caching in Program.cs
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddMemoryCache();

// For distributed caching:
// builder.Services.AddDistributedMemoryCache();
// Or for Redis:
// builder.Services.AddStackExchangeRedisCache(options =>
// {
//     options.Configuration = builder.Configuration.GetConnectionString("Redis");
//     options.InstanceName = "Example";
// });

// 3. Create a caching service
public interface ICacheService
{
    T Get<T>(string key);
    void Set<T>(string key, T value, TimeSpan? absoluteExpiration = null);
    void Remove(string key);
}

public class MemoryCacheService : ICacheService
{
    private readonly IMemoryCache _cache;
    private readonly ILogger<MemoryCacheService> _logger;
    
    // Default expiration time of 15 minutes
    private readonly TimeSpan _defaultExpiration = TimeSpan.FromMinutes(15);

    public MemoryCacheService(IMemoryCache cache, ILogger<MemoryCacheService> logger)
    {
        _cache = cache;
        _logger = logger;
    }

    public T Get<T>(string key)
    {
        if (_cache.TryGetValue(key, out T cachedValue))
        {
            _logger.LogDebug("Cache hit for key: {Key}", key);
            return cachedValue;
        }
        
        _logger.LogDebug("Cache miss for key: {Key}", key);
        return default;
    }

    public void Set<T>(string key, T value, TimeSpan? absoluteExpiration = null)
    {
        var expirationTime = absoluteExpiration ?? _defaultExpiration;
        
        var cacheOptions = new MemoryCacheEntryOptions()
            .SetAbsoluteExpiration(expirationTime)
            .SetPriority(CacheItemPriority.Normal);
            
        _cache.Set(key, value, cacheOptions);
        
        _logger.LogDebug("Cache set for key: {Key} with expiration: {Expiration}", key, expirationTime);
    }

    public void Remove(string key)
    {
        _cache.Remove(key);
        _logger.LogDebug("Cache removed for key: {Key}", key);
    }
}

// 4. Register the cache service
builder.Services.AddSingleton<ICacheService, MemoryCacheService>();

// 5. Example of using caching in a service
public class CachedProductService
{
    private readonly IProductRepository _repository;
    private readonly ICacheService _cacheService;
    private readonly ILogger<CachedProductService> _logger;

    public CachedProductService(
        IProductRepository repository,
        ICacheService cacheService,
        ILogger<CachedProductService> logger)
    {
        _repository = repository;
        _cacheService = cacheService;
        _logger = logger;
    }

    public async Task<Product> GetByIdAsync(int id)
    {
        string cacheKey = $"product:{id}";
        
        // Try to get from cache first
        var cachedProduct = _cacheService.Get<Product>(cacheKey);
        
        if (cachedProduct != null)
        {
            return cachedProduct;
        }
        
        // Cache miss, get from repository
        var product = await _repository.GetByIdAsync(id);
        
        if (product != null)
        {
            // Cache the product with a 15-minute expiration
            _cacheService.Set(cacheKey, product, TimeSpan.FromMinutes(15));
        }
        
        return product;
    }

    public async Task<IEnumerable<Product>> GetAllAsync()
    {
        string cacheKey = "products:all";
        
        // Try to get from cache first
        var cachedProducts = _cacheService.Get<IEnumerable<Product>>(cacheKey);
        
        if (cachedProducts != null)
        {
            return cachedProducts;
        }
        
        // Cache miss, get from repository
        var products = await _repository.GetAllAsync();
        
        // Cache with a shorter expiration since the full list might change more frequently
        _cacheService.Set(cacheKey, products, TimeSpan.FromMinutes(5));
        
        return products;
    }

    public async Task UpdateAsync(Product product)
    {
        await _repository.UpdateAsync(product);
        
        // Invalidate cache
        _cacheService.Remove($"product:{product.Id}");
        _cacheService.Remove("products:all");
    }
}
"""
    
    def _generate_validation_implementation(self) -> str:
        """Generate validation implementation code"""
        return """
// 1. Add this to your .csproj file
// <ItemGroup>
//   <PackageReference Include="FluentValidation.AspNetCore" Version="11.0.0" />
// </ItemGroup>

// 2. Register FluentValidation in Program.cs
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers()
    .AddFluentValidation(fv => fv.RegisterValidatorsFromAssemblyContaining<Program>());

// 3. Create validator classes
public class ProductValidator : AbstractValidator<ProductDto>
{
    public ProductValidator()
    {
        RuleFor(p => p.Name)
            .NotEmpty().WithMessage("Name is required")
            .MaximumLength(100).WithMessage("Name cannot exceed 100 characters");
            
        RuleFor(p => p.Price)
            .GreaterThan(0).WithMessage("Price must be greater than zero");
            
        RuleFor(p => p.CategoryId)
            .NotEmpty().WithMessage("Category is required");
            
        // Conditional validation example
        When(p => p.IsOnSale, () => {
            RuleFor(p => p.SalePrice)
                .NotNull().WithMessage("Sale price is required when product is on sale")
                .GreaterThan(0).WithMessage("Sale price must be greater than zero")
                .LessThan(p => p.Price).WithMessage("Sale price must be less than regular price");
        });
    }
}

// 4. Example of a DTO with Data Annotations (alternative approach)
public class CustomerDto
{
    [Required(ErrorMessage = "Name is required")]
    [StringLength(100, ErrorMessage = "Name cannot exceed 100 characters")]
    public string Name { get; set; }
    
    [Required(ErrorMessage = "Email is required")]
    [EmailAddress(ErrorMessage = "Invalid email format")]
    public string Email { get; set; }
    
    [Phone(ErrorMessage = "Invalid phone number format")]
    public string Phone { get; set; }
    
    [Range(18, 120, ErrorMessage = "Age must be between 18 and 120")]
    public int Age { get; set; }
}

// 5. Example of validation in a controller using FluentValidation
[ApiController]
[Route("api/[controller]")]
public class ProductsController : ControllerBase
{
    private readonly IProductService _productService;
    
    public ProductsController(IProductService productService)
    {
        _productService = productService;
    }
    
    [HttpPost]
    public async Task<IActionResult> Create(ProductDto productDto)
    {
        // ModelState validation is handled automatically with [ApiController]
        // If validation fails, a 400 Bad Request is returned with validation errors
        
        var product = await _productService.CreateAsync(productDto);
        return CreatedAtAction(nameof(GetById), new { id = product.Id }, product);
    }
}

// 6. Example of manual validation in a service
public class OrderService
{
    private readonly IValidator<OrderDto> _validator;
    
    public OrderService(IValidator<OrderDto> validator)
    {
        _validator = validator;
    }
    
    public async Task<Order> CreateOrderAsync(OrderDto orderDto)
    {
        // Validate the order
        var validationResult = await _validator.ValidateAsync(orderDto);
        
        if (!validationResult.IsValid)
        {
            var errors = validationResult.Errors.Select(e => new ValidationError 
            { 
                PropertyName = e.PropertyName, 
                ErrorMessage = e.ErrorMessage 
            });
            
            throw new ValidationException("Order validation failed", errors);
        }
        
        // Process the valid order
        var order = new Order
        {
            // Map properties from DTO
        };
        
        // Save the order
        
        return order;
    }
}
"""
    
    def _run(self, code: str) -> str:
        """
        Analyze cross-cutting concerns implementation and suggest improvements
        """
        # Analyze various cross-cutting concerns
        logging_analysis = self._analyze_logging(code)
        error_handling_analysis = self._analyze_error_handling(code)
        caching_analysis = self._analyze_caching(code)
        validation_analysis = self._analyze_validation(code)
        
        # Identify missing or inadequate concerns
        missing_concerns = []
        
        if not logging_analysis["logging_framework_detected"]:
            missing_concerns.append("logging")
        
        if not error_handling_analysis["global_error_handling"]:
            missing_concerns.append("error_handling")
        
        if not caching_analysis["caching_detected"]:
            missing_concerns.append("caching")
        
        if not validation_analysis["validation_approach"]:
            missing_concerns.append("validation")
        
        # Generate implementation code for missing concerns
        implementations = self._generate_cross_cutting_implementation(missing_concerns)
        
        # Prepare comprehensive analysis report
        analysis_report = {
            "logging": logging_analysis,
            "error_handling": error_handling_analysis,
            "caching": caching_analysis,
            "validation": validation_analysis,
            "missing_concerns": missing_concerns,
            "implementations": implementations
        }
        
        return json.dumps(analysis_report, indent=2)

class DatabaseMigrationTool(Tool):
    name = "DatabaseMigrationTool"
    description = "Generates and manages database migration scripts for schema changes"
    
    def _extract_entity_models(self, code: str) -> List[Dict[str, Any]]:
        """Extract entity models from code"""
        entities = []
        
        # Look for class definitions that might be entity models
        class_pattern = r'public\s+class\s+(\w+)(?:\s*:\s*\w+)?\s*\{([^}]*)\}'
        class_matches = re.findall(class_pattern, code, re.DOTALL)
        
        for class_name, class_body in class_matches:
            # Skip obvious non-entity classes
            if class_name.endswith("Controller") or class_name.endswith("Service") or class_name.endswith("Factory"):
                continue
            
            # Look for properties that might indicate an entity
            property_pattern = r'public\s+([\w<>[\],\s]+)\s+(\w+)\s*{\s*get;\s*set;\s*}'
            properties = re.findall(property_pattern, class_body)
            
            if not properties:
                continue
            
            # Check if it has an Id property or key attribute
            has_id = any(prop_name == "Id" for _, prop_name in properties)
            has_key = "[Key]" in class_body
            
            if has_id or has_key:
                entity = {
                    "name": class_name,
                    "properties": []
                }
                
                # Process properties
                for prop_type, prop_name in properties:
                    property_info = {
                        "name": prop_name,
                        "type": prop_type.strip(),
                        "nullable": "?" in prop_type.strip(),
                        "attributes": []
                    }
                    
                    # Extract property attributes
                    attr_pattern = r'\[([^\]]+)\]\s*public\s+([\w<>[\],\s]+)\s+' + re.escape(prop_name)
                    attr_matches = re.findall(attr_pattern, class_body)
                    
                    if attr_matches:
                        for attr_match in attr_matches:
                            attribute = attr_match[0]
                            property_info["attributes"].append(attribute)
                    
                    entity["properties"].append(property_info)
                
                entities.append(entity)
        
        return entities
    
    def _extract_dbcontext(self, code: str) -> Dict[str, Any]:
        """Extract DbContext configuration from code"""
        context_info = {
            "name": None,
            "db_sets": [],
            "configurations": []
        }
        
        # Look for DbContext class
        context_pattern = r'public\s+class\s+(\w+)\s*:\s*DbContext\s*\{([^}]*)\}'
        context_matches = re.findall(context_pattern, code, re.DOTALL)
        
        if not context_matches:
            return context_info
        
        context_name, context_body = context_matches[0]
        context_info["name"] = context_name
        
        # Extract DbSet properties
        dbset_pattern = r'public\s+DbSet<(\w+)>\s+(\w+)\s*{\s*get;\s*set;\s*}'
        dbset_matches = re.findall(dbset_pattern, context_body)
        
        for entity_type, set_name in dbset_matches:
            context_info["db_sets"].append({
                "entity_type": entity_type,
                "set_name": set_name
            })
        
        # Look for entity configurations
        config_pattern = r'modelBuilder\.Entity<(\w+)>\(\)\s*\.((?:[^;}]|;(?=\s*\.))*)'
        config_matches = re.findall(config_pattern, code, re.DOTALL)
        
        for entity_type, config_body in config_matches:
            configuration = {
                "entity_type": entity_type,
                "configurations": []
            }
            
            # Extract individual configurations
            for config_line in config_body.strip().split('\n'):
                config_line = config_line.strip()
                if config_line and not config_line.startswith("//"):
                    configuration["configurations"].append(config_line)
            
            context_info["configurations"].append(configuration)
        
        return context_info
    
    def _generate_migration_script(self, new_entities: List[Dict[str, Any]], db_context: Dict[str, Any], db_engine: str) -> str:
        """Generate migration script for the detected entities"""
        migration_script = f"""
// Entity Framework Core Migration Script
// Generated for {db_engine}

// 1. First, add Microsoft.EntityFrameworkCore.Tools package to your project:
// <PackageReference Include="Microsoft.EntityFrameworkCore.Tools" Version="6.0.0" />

// 2. Add the appropriate database provider:
// For SQL Server:
// <PackageReference Include="Microsoft.EntityFrameworkCore.SqlServer" Version="6.0.0" />
// For Oracle:
// <PackageReference Include="Oracle.EntityFrameworkCore" Version="6.0.0" />

// 3. Execute the following commands in Package Manager Console:

// To create a new migration:
// Add-Migration InitialCreate -Context {db_context.get('name', 'ApplicationDbContext')}

// To apply the migration to the database:
// Update-Database -Context {db_context.get('name', 'ApplicationDbContext')}

// 4. Alternatively, use the .NET CLI:
// dotnet ef migrations add InitialCreate --context {db_context.get('name', 'ApplicationDbContext')}
// dotnet ef database update --context {db_context.get('name', 'ApplicationDbContext')}

// DbContext Example:
public class {db_context.get('name', 'ApplicationDbContext')} : DbContext
{
    public {db_context.get('name', 'ApplicationDbContext')}(DbContextOptions<{db_context.get('name', 'ApplicationDbContext')}> options)
        : base(options)
    {
    }

"""
        
        # Add DbSet properties
        for entity in new_entities:
            migration_script += f"    public DbSet<{entity['name']}> {entity['name']}s {{ get; set; }}\n"
        
        migration_script += "\n    protected override void OnModelCreating(ModelBuilder modelBuilder)\n    {\n"
        
        # Add entity configurations
        for entity in new_entities:
            migration_script += f"        modelBuilder.Entity<{entity['name']}>(entity =>\n        {{\n"
            
            # Add primary key configuration
            id_property = next((p for p in entity["properties"] if p["name"] == "Id"), None)
            if id_property:
                migration_script += f"            entity.HasKey(e => e.Id);\n"
            
            # Add property configurations
            for prop in entity["properties"]:
                if any("Required" in attr for attr in prop["attributes"]):
                    migration_script += f"            entity.Property(e => e.{prop['name']}).IsRequired();\n"
                
                if any("StringLength" in attr for attr in prop["attributes"]):
                    # Extract string length from attribute
                    string_length_attr = next((attr for attr in prop["attributes"] if "StringLength" in attr), None)
                    if string_length_attr:
                        length_match = re.search(r'StringLength\((\d+)\)', string_length_attr)
                        if length_match:
                            length = length_match.group(1)
                            migration_script += f"            entity.Property(e => e.{prop['name']}).HasMaxLength({length});\n"
                
                # Add database-specific configurations
                if db_engine.lower() == "sqlserver":
                    if prop["type"] == "string" or prop["type"].startswith("string"):
                        migration_script += f"            entity.Property(e => e.{prop['name']}).HasColumnType(\"nvarchar(max)\");\n"
                    elif prop["type"] == "decimal" or prop["type"].startswith("decimal"):
                        migration_script += f"            entity.Property(e => e.{prop['name']}).HasColumnType(\"decimal(18,2)\");\n"
                elif db_engine.lower() == "oracle":
                    if prop["type"] == "string" or prop["type"].startswith("string"):
                        migration_script += f"            entity.Property(e => e.{prop['name']}).HasColumnType(\"NVARCHAR2(2000)\");\n"
                    elif prop["type"] == "decimal" or prop["type"].startswith("decimal"):
                        migration_script += f"            entity.Property(e => e.{prop['name']}).HasColumnType(\"NUMBER(18,2)\");\n"
            
            migration_script += "        });\n\n"
        
        migration_script += "    }\n}\n"
        
        # Add migration notes
        migration_script += """
// Important Notes:
// 1. Always review generated migrations before applying them to production databases
// 2. Consider creating seed data in migrations for reference or lookup tables
// 3. For complex changes, consider using Fluent API in the DbContext.OnModelCreating method

// Example of a data seeding operation:
// modelBuilder.Entity<Category>().HasData(
//     new Category { Id = 1, Name = "Electronics" },
//     new Category { Id = 2, Name = "Books" }
// );

// Example of adding an index:
// entity.HasIndex(e => e.Email).IsUnique();

// Example of configuring a relationship:
// modelBuilder.Entity<Order>()
//     .HasOne(o => o.Customer)
//     .WithMany(c => c.Orders)
//     .HasForeignKey(o => o.CustomerId);
"""
        
        return migration_script
    
    def _generate_schema_change_script(self, old_entities: List[Dict[str, Any]], new_entities: List[Dict[str, Any]], db_engine: str) -> str:
        """Generate migration script for schema changes between old and new entities"""
        # Find entities that exist in both old and new
        common_entity_names = set(e["name"] for e in old_entities).intersection(set(e["name"] for e in new_entities))
        
        # Find new entities
        new_entity_names = set(e["name"] for e in new_entities) - set(e["name"] for e in old_entities)
        
        # Find removed entities
        removed_entity_names = set(e["name"] for e in old_entities) - set(e["name"] for e in new_entities)
        
        # Generate migration script
        migration_script = f"// Schema Change Migration Script for {db_engine}\n\n"
        
        # Add new tables
        if new_entity_names:
            migration_script += "// New Tables\n"
            for entity_name in new_entity_names:
                entity = next(e for e in new_entities if e["name"] == entity_name)
                
                if db_engine.lower() == "sqlserver":
                    migration_script += f"""
CREATE TABLE [dbo].[{entity_name}s] (
"""
                elif db_engine.lower() == "oracle":
                    migration_script += f"""
CREATE TABLE {entity_name.upper()}S (
"""
                
                # Add columns
                columns = []
                for prop in entity["properties"]:
                    column_def = ""
                    if db_engine.lower() == "sqlserver":
                        column_type = self._get_sql_server_type(prop["type"])
                        nullability = "NULL" if prop["nullable"] else "NOT NULL"
                        
                        if prop["name"] == "Id":
                            column_def = f"    [Id] {column_type} IDENTITY(1,1) PRIMARY KEY"
                        else:
                            column_def = f"    [{prop['name']}] {column_type} {nullability}"
                    elif db_engine.lower() == "oracle":
                        column_type = self._get_oracle_type(prop["type"])
                        nullability = "NULL" if prop["nullable"] else "NOT NULL"
                        
                        if prop["name"] == "Id":
                            column_def = f"    ID {column_type} PRIMARY KEY"
                        else:
                            column_def = f"    {prop['name'].upper()} {column_type} {nullability}"
                    
                    columns.append(column_def)
                
                migration_script += ",\n".join(columns)
                
                if db_engine.lower() == "sqlserver":
                    migration_script += "\n);\nGO\n\n"
                elif db_engine.lower() == "oracle":
                    migration_script += "\n);\n/\n\n"
        
        # Modified tables
        if common_entity_names:
            migration_script += "// Modified Tables\n"
            for entity_name in common_entity_names:
                old_entity = next(e for e in old_entities if e["name"] == entity_name)
                new_entity = next(e for e in new_entities if e["name"] == entity_name)
                
                old_props = {p["name"]: p for p in old_entity["properties"]}
                new_props = {p["name"]: p for p in new_entity["properties"]}
                
                # Find added properties
                added_props = set(new_props.keys()) - set(old_props.keys())
                if added_props:
                    migration_script += f"-- Add columns to {entity_name}s table\n"
                    for prop_name in added_props:
                        prop = new_props[prop_name]
                        
                        if db_engine.lower() == "sqlserver":
                            column_type = self._get_sql_server_type(prop["type"])
                            nullability = "NULL" if prop["nullable"] else "NOT NULL"
                            migration_script += f"ALTER TABLE [dbo].[{entity_name}s] ADD [{prop_name}] {column_type} {nullability};\nGO\n"
                        elif db_engine.lower() == "oracle":
                            column_type = self._get_oracle_type(prop["type"])
                            nullability = "NULL" if prop["nullable"] else "NOT NULL"
                            migration_script += f"ALTER TABLE {entity_name.upper()}S ADD {prop_name.upper()} {column_type} {nullability};\n/\n"
                
                # Find removed properties
                removed_props = set(old_props.keys()) - set(new_props.keys())
                if removed_props:
                    migration_script += f"-- Drop columns from {entity_name}s table\n"
                    for prop_name in removed_props:
                        if db_engine.lower() == "sqlserver":
                            migration_script += f"ALTER TABLE [dbo].[{entity_name}s] DROP COLUMN [{prop_name}];\nGO\n"
                        elif db_engine.lower() == "oracle":
                            migration_script += f"ALTER TABLE {entity_name.upper()}S DROP COLUMN {prop_name.upper()};\n/\n"
                
                # Find modified properties
                common_props = set(old_props.keys()).intersection(set(new_props.keys()))
                for prop_name in common_props:
                    old_prop = old_props[prop_name]
                    new_prop = new_props[prop_name]
                    
                    if old_prop["type"] != new_prop["type"] or old_prop["nullable"] != new_prop["nullable"]:
                        migration_script += f"-- Modify column in {entity_name}s table\n"
                        
                        if db_engine.lower() == "sqlserver":
                            column_type = self._get_sql_server_type(new_prop["type"])
                            nullability = "NULL" if new_prop["                    "code_pattern": "POST/PUT/DELETE without ValidateAntiForgeryToken",
                    "match_count": 1
                })
        
        return vulnerabilities
    
    def _check_for_authorization_issues(self, code: str) -> List[Dict[str, Any]]:
        """Check for potential authorization issues"""
        vulnerabilities = []
        
        # Check for controllers without authorization
        if "Controller" in code and "public class" in code:
            # Look for controller class definitions
            controller_classes = re.findall(r'public\s+class\s+(\w+Controller)', code)
            
            if controller_classes:
                # Check if any authorization attributes are present
                auth_patterns = [
                    r'\[Authorize',
                    r'\[AllowAnonymous',
                    r'\[RequireHttps',
                ]
                
                auth_found = any(re.search(pattern, code) for pattern in auth_patterns)
                
                if not auth_found:
                    vulnerabilities.append({
                        "type": "missing_authorization",
                        "severity": "high",
                        "description": "Missing authorization: controller class without authorization attributes",
                        "remediation": "Add [Authorize] attributes to controllers or actions that require authentication",
                        "code_pattern": "Controller without [Authorize]",
                        "match_count": len(controller_classes)
                    })
        
        # Check for direct role strings instead of role constants
        role_string_patterns = [
            r'IsInRole\("(.*?)"\)',
            r'Authorize\(Roles = "(.*?)"\)',
        ]
        
        for pattern in role_string_patterns:
            matches = re.findall(pattern, code)
            if matches:
                vulnerabilities.append({
                    "type": "hardcoded_roles",
                    "severity": "low",
                    "description": "Hardcoded role strings: using string literals for role names instead of constants",
                    "remediation": "Use role constants or enums to avoid typos and improve maintainability",
                    "code_pattern": pattern,
                    "match_count": len(matches)
                })
        
        return vulnerabilities
    
    def _check_for_sensitive_data_exposure(self, code: str) -> List[Dict[str, Any]]:
        """Check for potential sensitive data exposure issues"""
        vulnerabilities = []
        
        # Check for sensitive data in model classes
        sensitive_property_patterns = [
            (r'(Password|Secret|ApiKey|Token|SSN|SocialSecurity|CreditCard|CardNumber)\b', "high"),
            (r'(Email|Phone|Address|DateOfBirth|BirthDate|DOB)\b', "medium"),
        ]
        
        for pattern, severity in sensitive_property_patterns:
            matches = re.findall(pattern, code)
            if matches:
                # Check if [DataProtection] or [Sensitive] attributes are used
                protection_patterns = [
                    r'\[(?:DataProtection|Sensitive|JsonIgnore|NotMapped)\]',
                    r'\[(?:ProtectedPersonalData|PersonalData)\]',
                ]
                
                protection_found = any(re.search(pattern, code) for pattern in protection_patterns)
                
                if not protection_found:
                    vulnerabilities.append({
                        "type": "sensitive_data_exposure",
                        "severity": severity,
                        "description": f"Potential sensitive data exposure: {', '.join(set(matches))} properties without proper protection attributes",
                        "remediation": "Add [JsonIgnore], [PersonalData], or encryption for sensitive data",
                        "code_pattern": pattern,
                        "match_count": len(matches)
                    })
        
        # Check for logging sensitive data
        logging_patterns = [
            r'Log\.(Debug|Info|Warning|Error|Critical)\([^)]*({0})[^)]*\)',
            r'(Debug|Info|Warning|Error|Critical)\([^)]*({0})[^)]*\)',
            r'logger\.(LogDebug|LogInformation|LogWarning|LogError|LogCritical)\([^)]*({0})[^)]*\)',
        ]
        
        sensitive_terms = "(Password|Secret|ApiKey|Token|SSN|SocialSecurity|CreditCard|CardNumber)"
        
        for base_pattern in logging_patterns:
            pattern = base_pattern.format(sensitive_terms)
            matches = re.findall(pattern, code)
            if matches:
                vulnerabilities.append({
                    "type": "sensitive_data_logging",
                    "severity": "high",
                    "description": "Potential sensitive data in logs: logging statements contain sensitive information",
                    "remediation": "Remove sensitive data from logging statements or redact it before logging",
                    "code_pattern": pattern,
                    "match_count": len(matches)
                })
        
        # Check for unsecured transmission (HTTP instead of HTTPS)
        if "http://" in code and not "https://" in code:
            vulnerabilities.append({
                "type": "unsecured_transmission",
                "severity": "medium",
                "description": "Unsecured data transmission: using HTTP instead of HTTPS",
                "remediation": "Use HTTPS for all external communication and add [RequireHttps] attribute",
                "code_pattern": "http://",
                "match_count": code.count("http://")
            })
        
        return vulnerabilities
    
    def _run(self, code: str) -> str:
        """
        Analyze code for security vulnerabilities and best practices
        """
        # Run various security checks
        sql_injection = self._check_for_sql_injection(code)
        xss = self._check_for_xss(code)
        csrf = self._check_for_csrf(code)
        authorization = self._check_for_authorization_issues(code)
        sensitive_data = self._check_for_sensitive_data_exposure(code)
        
        # Combine all vulnerabilities
        all_vulnerabilities = sql_injection + xss + csrf + authorization + sensitive_data
        
        # Count vulnerabilities by severity
        severity_counts = {
            "high": sum(1 for v in all_vulnerabilities if v["severity"] == "high"),
            "medium": sum(1 for v in all_vulnerabilities if v["severity"] == "medium"),
            "low": sum(1 for v in all_vulnerabilities if v["severity"] == "low")
        }
        
        # Generate overall security score (simple algorithm)
        # Start with 100 points and subtract for vulnerabilities
        security_score = 100
        security_score -= severity_counts["high"] * 20
        security_score -= severity_counts["medium"] * 10
        security_score -= severity_counts["low"] * 5
        
        # Ensure score doesn't go below 0
        security_score = max(0, security_score)
        
        # Generate security rating
        if security_score >= 90:
            security_rating = "Excellent"
        elif security_score >= 80:
            security_rating = "Good"
        elif security_score >= 70:
            security_rating = "Satisfactory"
        elif security_score >= 60:
            security_rating = "Needs Improvement"
        else:
            security_rating = "Poor"
        
        # Group vulnerabilities by type
        vulnerability_types = {}
        for vuln in all_vulnerabilities:
            vuln_type = vuln["type"]
            if vuln_type not in vulnerability_types:
                vulnerability_types[vuln_type] = []
            vulnerability_types[vuln_type].append(vuln)
        
        # Generate recommendations
        recommendations = []
        if security_score < 80:
            for vuln_type, vulns in vulnerability_types.items():
                highest_severity = max([v["severity"] for v in vulns], key=lambda s: {"high": 3, "medium": 2, "low": 1}[s])
                if highest_severity in ["high", "medium"]:
                    # Get a unique list of remediations
                    unique_remediations = list(set([v["remediation"] for v in vulns]))
                    remediation_text = "; ".join(unique_remediations)
                    recommendations.append(f"Fix {vuln_type} issues: {remediation_text}")
        
        # Combine all analysis into a comprehensive report
        security_report = {
            "security_score": security_score,
            "security_rating": security_rating,
            "vulnerability_counts": severity_counts,
            "vulnerabilities": all_vulnerabilities,
            "recommendations": recommendations
        }
        
        return json.dumps(security_report, indent=2)

class DependencyManagementTool(Tool):
    name = "DependencyManager"
    description = "Analyzes and optimizes dependencies and NuGet package configurations"
    
    def _extract_project_references(self, code: str) -> List[str]:
        """Extract project references from code or project files"""
        project_refs = []
        
        # Look for project reference patterns in csproj files
        project_ref_pattern = r'<ProjectReference\s+Include="([^"]+)"'
        project_refs.extend(re.findall(project_ref_pattern, code))
        
        # Also look for using statements in code files
        using_pattern = r'using\s+([\w\.]+);'
        using_statements = re.findall(using_pattern, code)
        
        # Filter out system/standard namespaces
        standard_namespaces = [
            "System", "Microsoft", "Newtonsoft", "Xunit", 
            "FluentValidation", "AutoMapper", "EntityFramework"
        ]
        
        for ns in using_statements:
            if not any(ns.startswith(std) for std in standard_namespaces):
                project_refs.append(ns)
        
        return list(set(project_refs))
    
    def _extract_nuget_packages(self, code: str) -> List[Dict[str, str]]:
        """Extract NuGet package references from code or project files"""
        packages = []
        
        # Look for package reference patterns in csproj files
        package_ref_pattern = r'<PackageReference\s+Include="([^"]+)"\s+Version="([^"]+)"'
        package_matches = re.findall(package_ref_pattern, code)
        
        for name, version in package_matches:
            packages.append({
                "name": name,
                "version": version,
                "source": "csproj"
            })
        
        # Look for packages.config
        packageconfig_pattern = r'<package\s+id="([^"]+)"\s+version="([^"]+)"'
        packageconfig_matches = re.findall(packageconfig_pattern, code)
        
        for name, version in packageconfig_matches:
            packages.append({
                "name": name,
                "version": version,
                "source": "packages.config"
            })
        
        return packages
    
    def _analyze_package_versions(self, packages: List[Dict[str, str]]) -> List[Dict[str, Any]]:
        """Analyze package versions for potential upgrades or conflicts"""
        analysis = []
        
        # Group packages by name
        package_groups = {}
        for package in packages:
            name = package["name"]
            if name not in package_groups:
                package_groups[name] = []
            package_groups[name].append(package)
        
        # Check for multiple versions of the same package
        for name, pkgs in package_groups.items():
            if len(pkgs) > 1 and len(set(p["version"] for p in pkgs)) > 1:
                versions = [p["version"] for p in pkgs]
                analysis.append({
                    "type": "version_conflict",
                    "package": name,
                    "versions": versions,
                    "recommendation": f"Standardize on the latest version: {max(versions, key=lambda v: [int(x) for x in v.split('.')])}"
                })
        
        # Check for outdated version patterns (this is simplified and would need a real package DB)
        for package in packages:
            version = package["version"]
            version_parts = version.split('.')
            
            # Check for packages still on major version 1 or older
            if version.startswith("0.") or version.startswith("1."):
                analysis.append({
                    "type": "outdated_major_version",
                    "package": package["name"],
                    "version": version,
                    "recommendation": "Consider upgrading to the latest major version for new features and security updates"
                })
            
            # Check for very old versions by simplistic heuristic
            if len(version_parts) >= 2 and int(version_parts[0]) <= 2 and int(version_parts[1]) <= 5:
                analysis.append({
                    "type": "potentially_outdated",
                    "package": package["name"],
                    "version": version,
                    "recommendation": "This appears to be an older version. Check for updates."
                })
        
        return analysis
    
    def _analyze_dependency_structure(self, project_refs: List[str], packages: List[Dict[str, str]]) -> Dict[str, Any]:
        """Analyze the dependency structure for potential improvements"""
        analysis = {
            "dependency_count": len(project_refs) + len(packages),
            "project_reference_count": len(project_refs),
            "package_reference_count": len(packages),
            "issues": []
        }
        
        # Check for high number of dependencies
        if analysis["dependency_count"] > 20:
            analysis["issues"].append({
                "type": "high_dependency_count",
                "description": f"High number of dependencies ({analysis['dependency_count']})",
                "recommendation": "Consider consolidating or refactoring to reduce the number of dependencies"
            })
        
        # Check for common architecture smells
        architecture_smells = []
        
        for ref in project_refs:
            parts = ref.split('.')
            if len(parts) >= 2:
                layer_hints = ['api', 'data', 'dal', 'repository', 'domain', 'model', 'service', 'business', 'ui']
                
                for i, part in enumerate(parts):
                    part_lower = part.lower()
                    if part_lower in layer_hints:
                        # Check for potential dependency direction issues
                        if part_lower in ['api', 'ui', 'presentation'] and any(p.lower() in ['data', 'dal', 'repository'] for p in parts):
                            architecture_smells.append({
                                "type": "layer_violation",
                                "reference": ref,
                                "description": "Potential architecture layer violation: presentation layer should not directly reference data layer",
                                "recommendation": "Consider introducing interfaces or using a mediator pattern"
                            })
                        
                        # Check for potentially missing abstraction layers
                        if part_lower in ['domain', 'model'] and any(p.lower() in ['data', 'dal', 'repository'] for p in parts):
                            architecture_smells.append({
                                "type": "missing_abstraction",
                                "reference": ref,
                                "description": "Domain models directly referencing data layer",
                                "recommendation": "Consider introducing an abstraction layer or repository interfaces"
                            })
        
        if architecture_smells:
            analysis["issues"].extend(architecture_smells)
        
        return analysis
    
    def _generate_package_recommendations(self, code: str, packages: List[Dict[str, str]]) -> List[Dict[str, str]]:
        """Generate recommendations for additional useful packages based on code content"""
        recommendations = []
        
        # Package to pattern mapping for detection
        package_patterns = {
            "Swashbuckle.AspNetCore": (r'(api|controller|swagger)', "API documentation"),
            "Microsoft.Extensions.Caching.Memory": (r'(cache|caching)', "In-memory caching"),
            "NLog" if not any(p["name"] == "Serilog" for p in packages) else None: (r'(log|logger|logging)', "Structured logging"),
            "FluentValidation.AspNetCore" if not any(p["name"].startswith("FluentValidation") for p in packages) else None: (r'(valid|validation|validator)', "Validation"),
            "AutoMapper.Extensions.Microsoft.DependencyInjection" if not any(p["name"].startswith("AutoMapper") for p in packages) else None: (r'(map|mapping)', "Object mapping"),
            "MediatR" if not any(p["name"] == "MediatR" for p in packages) else None: (r'(command|query|request|handler)', "CQRS pattern implementation"),
            "Microsoft.AspNetCore.Authentication.JwtBearer" if "token" in code.lower() or "jwt" in code.lower() else None: (r'(auth|authentication|jwt|token)', "JWT authentication"),
        }
        
        # Remove None entries
        package_patterns = {k: v for k, v in package_patterns.items() if k is not None}
        
        # Check each pattern
        for package, (pattern, description) in package_patterns.items():
            if re.search(pattern, code, re.IGNORECASE) and not any(p["name"] == package for p in packages):
                recommendations.append({
                    "package": package,
                    "reason": f"Code suggests need for {description}",
                    "description": description
                })
        
        return recommendations
    
    def _run(self, code: str) -> str:
        """
        Analyze and optimize dependencies and NuGet package configurations
        """
        # Extract project and package references
        project_refs = self._extract_project_references(code)
        packages = self._extract_nuget_packages(code)
        
        # Analyze package versions
        package_analysis = self._analyze_package_versions(packages)
        
        # Analyze dependency structure
        dependency_analysis = self._analyze_dependency_structure(project_refs, packages)
        
        # Generate package recommendations
        package_recommendations = self._generate_package_recommendations(code, packages)
        
        # Combine into a comprehensive report
        dependency_report = {
            "project_references": project_refs,
            "nuget_packages": packages,
            "package_version_analysis": package_analysis,
            "dependency_structure_analysis": dependency_analysis,
            "package_recommendations": package_recommendations
        }
        
        return json.dumps(dependency_report, indent=2)

class CrossCuttingConcernsTool(Tool):
    name = "CrossCuttingConcernsTool"
    description = "Analyzes and implements cross-cutting concerns like logging, error handling, and caching"
    
    def _analyze_logging(self, code: str) -> Dict[str, Any]:
        """Analyze logging implementation in the code"""
        analysis = {
            "logging_framework_detected": None,
            "logging_pattern": None,
            "logging_coverage": 0,
            "issues": [],
            "recommendations": []
        }
        
        # Detect logging framework
        logging_frameworks = {
            "Microsoft.Extensions.Logging": r'(ILogger|_logger\.|Logger\.|LogInformation|LogWarning|LogError)',
            "Serilog": r'(Serilog|Log\.)',
            "NLog": r'(NLog|LogManager|ILogger)',
            "log4net": r'(log4net|ILog)'
        }
        
        for framework, pattern in logging_frameworks.items():
            if re.search(pattern, code):
                analysis["logging_framework_detected"] = framework
                break
        
        # Detect logging pattern
        if analysis["logging_framework_detected"]:
            # Check for dependency injection pattern
            if re.search(r'(private\s+readonly\s+ILogger|private\s+readonly\s+ILog)', code):
                analysis["logging_pattern"] = "dependency_injection"
            # Check for static logger pattern
            elif re.search(r'(private\s+static\s+readonly\s+ILogger|private\s+static\s+readonly\s+ILog|private\s+static\s+readonly\s+Logger)', code):
                analysis["logging_pattern"] = "static_logger"
            # Check for direct logging pattern
            elif re.search(r'(Log\.|Logger\.)', code):
                analysis["logging_pattern"] = "direct_logging"
        
        # Estimate logging coverage
        if analysis["logging_framework_detected"]:
            # Count public methods
            public_methods = re.findall(r'public\s+(?:async\s+)?[\w<>[\],\s]+\s+\w+\s*\(', code)
            public_method_count = len(public_methods)
            
            # Count methods with logging
            log_pattern = r'(Log\.|Logger\.|_logger\.|LogInformation|LogWarning|LogError|Debug\(|Info\(|Warn\(|Error\()'
            methods_with_logging = re.findall(r'public\s+(?:async\s+)?[\w<>[\],\s]+\s+\w+\s*\([^{]*\{[^}]*' + log_pattern, code, re.DOTALL)
            methods_with_logging_count = len(methods_with_logging)
            
            # Calculate coverage percentage
            if public_method_count > 0:
                analysis["logging_coverage"] = round((methods_with_logging_count / public_method_count) * 100)
        
        # Identify issues and recommendations
        if not analysis["logging_framework_detected"]:
            analysis["issues"].append("No logging framework detected")
            analysis["recommendations"].append("Implement a logging framework (Microsoft.Extensions.Logging, Serilog, or NLog)")
        elif analysis["logging_coverage"] < 50:
            analysis["issues"].append(f"Low logging coverage ({analysis['logging_coverage']}%)")
            analysis["recommendations"].append("Increase logging coverage, especially for error conditions and key application events")
        
        if analysis["logging_pattern"] == "direct_logging":
            analysis["issues"].append("Using direct logging pattern which can be harder to mock or replace")
            analysis["recommendations"].append("Consider using dependency injection for logging to improve testability")
        
        # Check for structured logging
        if analysis["logging_framework_detected"] and not re.search(r'Log\w+\([^,]+,[^,]+\)', code):
            analysis["issues"].append("May not be using structured logging")
            analysis["recommendations"].append("Use structured logging with named parameters instead of string concatenation")
        
        return analysis
    
    def _analyze_error_handling(self, code: str) -> Dict[str, Any]:
        """Analyze error handling implementation in the code"""
        analysis = {
            "try_catch_blocks": 0,
            "global_error_handling": False,
            "custom_exceptions": [],
            "issues": [],
            "recommendations": []
        }
        
        # Count try-catch blocks
        try_catch_blocks = re.findall(r'try\s*\{[^}]*\}\s*catch', code, re.DOTALL)
        analysis["try_catch_blocks"] = len(try_catch_blocks)
        
        # Check for global exception handling
        global_handlers = [
            r'UseExceptionHandler',
            r'IExceptionFilter',
            r'ExceptionMiddleware',
            r'OnException',
            r'app\.Use\([^)]*Exception',
        ]
        
        for handler in global_handlers:
            if re.search(handler, code):
                analysis["global_error_handling"] = True
                break
        
        # Identify custom exceptions
        custom_exceptions = re.findall(r'class\s+(\w+Exception)\s*:\s*Exception', code)
        analysis["custom_exceptions"] = custom_exceptions
        
        # Identify issues and recommendations
        if not analysis["global_error_handling"]:
            analysis["issues"].append("No global exception handling detected")
            analysis["recommendations"].append("Implement global exception handling middleware to catch unhandled exceptions")
        
        # Check for empty catch blocks
        empty_catches = re.findall(r'catch[^{]*\{\s*\}', code)
        if empty_catches:
            analysis["issues"].append(f"Found {len(empty_catches)} empty catch blocks")
            analysis["recommendations"].append("Avoid empty catch blocks; at minimum, log the exception")
        
        # Check for general Exception catching
        general_catches = re.findall(r'catch\s*\(\s*Exception\b', code)
        if general_catches:
            analysis["issues"].append(f"Found {len(general_catches)} catch blocks catching general Exception type")
            analysis["recommendations"].append("Catch specific exception types when possible")
        
        # Check for exception swallowing
        if re.search(r'catch[^{]*\{[^}]*return', code, re.DOTALL):
            analysis["issues"].append("Potential exception swallowing detected (returning from catch blocks without proper handling)")
            analysis["recommendations"].append("Ensure exceptions are properly logged before returning from catch blocks")
        
        return analysis
    
    def _analyze_caching(self, code: str) -> Dict[str, Any]:
        """Analyze caching implementation in the code"""
        analysis = {
            "caching_detected": False,
            "caching_technology": None,
            "cache_regions": [],
            "issues": [],
            "recommendations": []
        }
        
        # Detect caching technology
        caching_technologies = {
            "Memory Cache": r'(IMemoryCache|MemoryCache|AddMemoryCache)',
            "Distributed Cache": r'(IDistributedCache|DistributedCache|AddDistributedCache)',
            "Redis": r'(ConnectionMultiplexer|StackExchange\.Redis|AddRedis)',
            "Output Caching": r'(ResponseCache|OutputCache|AddResponseCaching)',
            "Entity Framework Caching": r'(EF\s+Second\s+Level\s+Cache|EntityFrameworkCore\.Cacheable)'
        }
        
        for tech, pattern in caching_technologies.items():
            if re.search(pattern, code):
                analysis["caching_detected"] = True
                analysis["caching_technology"] = tech
                break
        
        # Identify cache regions/keys
        if analysis["caching_detected"]:
            cache_keys = re.findall(r'(?:Get|Set|Remove|TryGetValue)\(\s*"([^"]+)"', code)
            analysis["cache_regions"] = list(set(cache_keys))
        
        # Check for cacheable methods
        if not analysis["caching_detected"]:
            # Look for potentially cacheable read operations
            potential_cacheable = re.findall(r'public\s+(?:async\s+)?[\w<>[\],\s]+\s+Get\w+\s*\([^)]*\)', code)
            if potential_cacheable:
                analysis["issues"].append("No caching detected but found potentially cacheable methods")
                analysis["recommendations"].append("Consider implementing caching for read-heavy operations")
        else:
            # Check for cache expiration
            if not re.search(r'TimeSpan|ExpirationScanFrequency|AbsoluteExpiration|SlidingExpiration', code):
                analysis["issues"].append("Caching implemented but no explicit expiration policy detected")
                analysis["recommendations"].append("Set appropriate cache expiration policies to avoid stale data")
        
        return analysis
    
    def _analyze_validation(self, code: str) -> Dict[str, Any]:
        """Analyze validation implementation in the code"""
        analysis = {
            "validation_approach": None,
            "validation_coverage": 0,
            "issues": [],
            "recommendations": []
        }
        
        # Detect validation approach
        validation_approaches = {
            "Data Annotations": r'(\[Required\]|\[StringLength\]|\[Range\]|\[RegularExpression\])',
            "FluentValidation": r'(AbstractValidator|RuleFor|ValidationResult)',
            "Manual Validation": r'(if\s*\([^)]*==\s*null|if\s*\([^)]*\.Length|ModelState\.IsValid)',
            "Guard Clauses": r'(Guard\.|Throw\.|ArgumentNullException)'
        }
        
        for approach, pattern in validation_approaches.items():
            if re.search(pattern, code):
                analysis["validation_approach"] = approach
                break
        
        # Estimate validation coverage
        if analysis["validation_approach"]:
            # Count public methods with parameters
            public_methods = re.findall(r'public\s+(?:async\s+)?[\w<>[\],\s]+\s+\w+\s*\([^)]+\)', code)
            public_method_count = len(public_methods)
            
            # Count methods with validation
            validation_pattern = r'(ModelState\.IsValid|RuleFor|Required|StringLength|if\s*\([^)]*==\s*null)'
            methods_with_validation = re.findall(r'public\s+(?:async\s+)?[\w<>[\],\s]+\s+\w+\s*\([^)]+\)[^{]*\{[^}]*' + validation_pattern, code, re.DOTALL)
            methods_with_validation_count = len(methods_with_validation)
            
            # Calculate coverage percentage
            if public_method_count > 0:
                analysis["validation_coverage"] = round((methods_with_validation_count / public_method_count) * 100)
        
        # Identify issues and recommendations
        if not analysis["validation_approach"]:
            analysis["issues"].append("No structured validation approach detected")
            analysis["recommendations"].append("Implement a consistent validation approach (FluentValidation or Data Annotations recommended)")
        elif analysis["validation_coverage"] < 50:
            analysis["issues"].append(f"Low validation coverage ({analysis['validation_coverage']}%)")
            analysis["recommendations"].append("Increase validation coverage for all input parameters")
        
        if analysis["validation_approach"] == "Manual Validation":
            analysis["issues"].append("Using manual validation which can be inconsistent and error-prone")
            analysis["recommendations"].append("Consider using a structured validation approach like FluentValidation or Data Annotations")
        
        # Check for validation error handling
        if analysis["validation_approach"] and not re.search(r'(ModelState\.IsValid|ValidationResult|TryValidateModel)', code):
            analysis["issues"].append("Validation may be implemented but no proper error handling detected")
            analysis["recommendations"].append("Ensure validation errors are properly returned to clients")
        
        return analysis
    
    def _generate_cross_cutting_implementation(self, missing_concerns: List[str]) -> Dict[str, str]:
        """Generate implementation code for missing cross-cutting concerns"""
        implementations = {}
        
        for concern in missing_concerns:
            if concern == "logging":
                implementations["logging"] = self._generate_logging_implementation()
            elif concern == "error_handling":
                implementations["error_handling"] = self._generate_error_handling_implementation()
            elif concern == "caching":
                implementations["caching"] = self._generate_caching_implementation()
            elif concern == "validation":
                implementations["validation"] = self._generate_validation_implementation()
        
        return implementations
    
    def _generate_logging_implementation(self) -> str:
        """Generate logging implementation code"""
        return """
// 1. Add this to your .csproj file
// <ItemGroup>
//   <PackageReference Include="Microsoft.Extensions.Logging" Version="6.0.0" />
//   <PackageReference Include="Serilog.AspNetCore" Version="6.0.0" />
//   <PackageReference Include="Serilog.Sinks.Console" Version="4.1.0" />
//   <PackageReference Include="Serilog.Sinks.File" Version="5.0.0" />
// </ItemGroup>

// 2. Add this to Program.cs
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Configure S            elif lines > 15:
                smells.append({
                    "type": "long_method",
                    "description": f"Method '{method_match[3]}' has {lines} lines. Consider if it could be simplified.",
                    "severity": "medium"
                })
        
        # Check for large classes
        class_pattern = r'class\s+(\w+)[^{]*\{([^}]*)\}'
        classes = re.findall(class_pattern, code, re.DOTALL)
        for class_match in classes:
            class_body = class_match[1]
            method_count = len(re.findall(method_pattern, class_body, re.DOTALL))
            if method_count > 20:
                smells.append({
                    "type": "large_class",
                    "description": f"Class '{class_match[0]}' has {method_count} methods. Consider breaking it down into smaller classes.",
                    "severity": "high"
                })
            elif method_count > 10:
                smells.append({
                    "type": "large_class",
                    "description": f"Class '{class_match[0]}' has {method_count} methods. Consider if it has too many responsibilities.",
                    "severity": "medium"
                })
        
        # Check for commented out code
        comment_pattern = r'//.*'
        comments = re.findall(comment_pattern, code)
        code_like_comments = [c for c in comments if ';' in c or '{' in c or '}' in c]
        if len(code_like_comments) > 3:
            smells.append({
                "type": "commented_code",
                "description": f"Found {len(code_like_comments)} potential instances of commented-out code. This should be removed.",
                "severity": "low"
            })
        
        # Check for string concatenation in loops
        string_concat_in_loops = re.findall(r'(for|foreach|while)[^{]*\{[^}]*\+=[^}]*\}', code, re.DOTALL)
        if string_concat_in_loops:
            smells.append({
                "type": "string_concatenation_in_loop",
                "description": "Found string concatenation in loops. Consider using StringBuilder for better performance.",
                "severity": "medium"
            })
        
        return smells
    
    def _analyze_best_practices(self, code: str) -> List[Dict[str, str]]:
        """Check adherence to C# best practices"""
        best_practices = []
        
        # Check for async/await usage
        async_methods = re.findall(r'async\s+[\w<>[\],\s]+\s+\w+\s*\(', code)
        await_usage = re.findall(r'await\s+', code)
        if async_methods and not await_usage:
            best_practices.append({
                "type": "async_without_await",
                "description": "Methods marked as async but no await operators found. This may indicate missing await statements.",
                "recommendation": "Ensure all async methods use await or remove the async modifier if not needed."
            })
        
        # Check for proper disposal of IDisposable objects
        disposable_patterns = [
            r'new\s+SqlConnection',
            r'new\s+StreamReader',
            r'new\s+StreamWriter',
            r'new\s+FileStream',
            r'new\s+MemoryStream'
        ]
        
        using_statements = re.findall(r'using\s*\(', code)
        for pattern in disposable_patterns:
            disposable_objects = re.findall(pattern, code)
            if disposable_objects and len(disposable_objects) > len(using_statements):
                best_practices.append({
                    "type": "disposable_not_disposed",
                    "description": f"Potential improper disposal of IDisposable objects ({pattern.replace('new\\s+', '')})",
                    "recommendation": "Use 'using' statements or blocks to ensure proper disposal of resources."
                })
        
        # Check for proper exception handling
        catch_blocks = re.findall(r'catch\s*\([^)]*\)\s*\{([^}]*)\}', code, re.DOTALL)
        general_exceptions = re.findall(r'catch\s*\(\s*Exception\s+', code)
        empty_catches = [b for b in catch_blocks if not b.strip()]
        
        if empty_catches:
            best_practices.append({
                "type": "empty_catch_block",
                "description": f"Found {len(empty_catches)} empty catch blocks. This suppresses exceptions without handling them.",
                "recommendation": "Handle exceptions properly or rethrow them if they cannot be handled at this level."
            })
        
        if general_exceptions and len(general_exceptions) == len(catch_blocks):
            best_practices.append({
                "type": "catching_general_exception",
                "description": "All catch blocks are catching general Exception type. This may hide unexpected exceptions.",
                "recommendation": "Catch specific exception types when possible to handle each case appropriately."
            })
        
        # Check for proper validation
        controller_methods = re.findall(r'(public|private|protected|internal)\s+(async\s+)?([\w<>[\],\s]+)\s+(\w+)\s*\([^)]*\)\s*\{([^}]*)\}', code, re.DOTALL)
        model_state_checks = re.findall(r'ModelState\.IsValid', code)
        
        if "Controller" in code and controller_methods and not model_state_checks:
            best_practices.append({
                "type": "missing_model_validation",
                "description": "Controller methods may be missing ModelState validation checks.",
                "recommendation": "Add ModelState.IsValid checks to validate request data before processing."
            })
        
        return best_practices
    
    def _run(self, code: str) -> str:
        """
        Analyze code quality against best practices and coding standards
        """
        # Run various analysis methods
        complexity_analysis = self._analyze_code_complexity(code)
        naming_issues = self._check_naming_conventions(code)
        code_smells = self._check_code_smells(code)
        best_practices_issues = self._analyze_best_practices(code)
        
        # Determine overall quality score (simple algorithm)
        # Start with 100 points and subtract for issues
        quality_score = 100
        
        # Complexity penalties
        if complexity_analysis["cyclomatic_complexity"] > 30:
            quality_score -= 15
        elif complexity_analysis["cyclomatic_complexity"] > 15:
            quality_score -= 5
        
        if complexity_analysis["nesting_depth"] > 5:
            quality_score -= 15
        elif complexity_analysis["nesting_depth"] > 3:
            quality_score -= 5
        
        if complexity_analysis["method_length"] > 50:
            quality_score -= 15
        elif complexity_analysis["method_length"] > 30:
            quality_score -= 5
        
        # Naming convention penalties
        naming_count = sum(len(issues) for issues in naming_issues.values())
        if naming_count > 10:
            quality_score -= 15
        elif naming_count > 5:
            quality_score -= 10
        elif naming_count > 0:
            quality_score -= 5
        
        # Code smell penalties
        high_severity_smells = [smell for smell in code_smells if smell["severity"] == "high"]
        medium_severity_smells = [smell for smell in code_smells if smell["severity"] == "medium"]
        low_severity_smells = [smell for smell in code_smells if smell["severity"] == "low"]
        
        quality_score -= len(high_severity_smells) * 10
        quality_score -= len(medium_severity_smells) * 5
        quality_score -= len(low_severity_smells) * 2
        
        # Best practices penalties
        quality_score -= len(best_practices_issues) * 5
        
        # Ensure score doesn't go below 0
        quality_score = max(0, quality_score)
        
        # Generate quality rating
        if quality_score >= 90:
            quality_rating = "Excellent"
        elif quality_score >= 80:
            quality_rating = "Good"
        elif quality_score >= 70:
            quality_rating = "Satisfactory"
        elif quality_score >= 60:
            quality_rating = "Needs Improvement"
        else:
            quality_rating = "Poor"
        
        # Combine all analysis into a comprehensive report
        analysis_report = {
            "quality_score": quality_score,
            "quality_rating": quality_rating,
            "complexity_metrics": complexity_analysis,
            "naming_convention_issues": naming_issues,
            "code_smells": code_smells,
            "best_practices_issues": best_practices_issues,
            "recommendations": []
        }
        
        # Generate top recommendations
        if complexity_analysis["cyclomatic_complexity"] > 15:
            analysis_report["recommendations"].append("Reduce code complexity by breaking down complex methods")
        
        if complexity_analysis["nesting_depth"] > 3:
            analysis_report["recommendations"].append("Reduce nesting depth by extracting code to separate methods")
        
        if naming_count > 0:
            analysis_report["recommendations"].append("Follow .NET naming conventions consistently")
        
        if high_severity_smells:
            analysis_report["recommendations"].append("Address high-severity code smells: " + ", ".join(set(smell["type"] for smell in high_severity_smells)))
        
        if best_practices_issues:
            analysis_report["recommendations"].append("Follow best practices: " + ", ".join(set(issue["type"] for issue in best_practices_issues)))
        
        return json.dumps(analysis_report, indent=2)

class ApiTestingTool(Tool):
    name = "ApiTester"
    description = "Generates and validates API tests for thoroughness and coverage"
    
    def _generate_api_test_cases(self, endpoint: Dict[str, Any]) -> List[Dict[str, Any]]:
        """Generate test cases for an API endpoint"""
        test_cases = []
        
        method = endpoint.get("method", "GET")
        path = endpoint.get("endpoint", "/api/resource")
        action = endpoint.get("action", "")
        entity = endpoint.get("entity", "resource")
        
        # Happy path test case
        happy_path = {
            "description": f"Valid {action} operation on {entity}",
            "request": {
                "method": method,
                "path": path.replace("{id}", "1"),  # Replace path parameters with sample values
                "headers": {
                    "Content-Type": "application/json",
                    "Accept": "application/json"
                },
                "query_params": {},
                "body": None
            },
            "expected_response": {
                "status_code": self._get_expected_status_code(method, action),
                "content_type": "application/json",
                "body_contains": [],
                "validation_rules": []
            }
        }
        
        # Add request body for methods that require it
        if method in ["POST", "PUT", "PATCH"]:
            happy_path["request"]["body"] = self._generate_sample_request_body(entity)
            
            # Add validation rules
            if method == "POST":
                happy_path["expected_response"]["body_contains"] = ["id"]
                happy_path["expected_response"]["validation_rules"].append("Response should contain an ID for the created resource")
        
        test_cases.append(happy_path)
        
        # Add common edge cases based on the action
        if action.lower() in ["create", "update"]:
            # Validation failure test
            validation_failure = {
                "description": f"Invalid {action} operation with missing required fields",
                "request": {
                    "method": method,
                    "path": path.replace("{id}", "1"),
                    "headers": {
                        "Content-Type": "application/json",
                        "Accept": "application/json"
                    },
                    "query_params": {},
                    "body": {}  # Empty body to trigger validation errors
                },
                "expected_response": {
                    "status_code": 400,
                    "content_type": "application/json",
                    "body_contains": ["error", "validation"],
                    "validation_rules": ["Response should contain validation error details"]
                }
            }
            test_cases.append(validation_failure)
        
        if action.lower() in ["read", "update", "delete"]:
            # Not found test
            not_found = {
                "description": f"{action} operation with non-existent ID",
                "request": {
                    "method": method,
                    "path": path.replace("{id}", "99999"),  # Use a likely non-existent ID
                    "headers": {
                        "Accept": "application/json"
                    },
                    "query_params": {},
                    "body": None
                },
                "expected_response": {
                    "status_code": 404,
                    "validation_rules": ["Response should indicate resource not found"]
                }
            }
            
            if method == "PUT":
                not_found["request"]["body"] = self._generate_sample_request_body(entity)
                not_found["request"]["body"]["id"] = 99999  # Ensure ID in body matches path
            
            test_cases.append(not_found)
        
        if action.lower() == "update":
            # ID mismatch test
            id_mismatch = {
                "description": f"{action} operation with mismatched IDs in path and body",
                "request": {
                    "method": method,
                    "path": path.replace("{id}", "1"),
                    "headers": {
                        "Content-Type": "application/json",
                        "Accept": "application/json"
                    },
                    "query_params": {},
                    "body": self._generate_sample_request_body(entity)
                },
                "expected_response": {
                    "status_code": 400,
                    "validation_rules": ["Response should indicate ID mismatch error"]
                }
            }
            id_mismatch["request"]["body"]["id"] = 2  # Different from path ID
            test_cases.append(id_mismatch)
        
        if action.lower() == "list" or action.lower() == "search":
            # Pagination test
            pagination_test = {
                "description": f"{action} operation with pagination parameters",
                "request": {
                    "method": method,
                    "path": path,
                    "headers": {
                        "Accept": "application/json"
                    },
                    "query_params": {
                        "pageNumber": "2",
                        "pageSize": "10"
                    },
                    "body": None
                },
                "expected_response": {
                    "status_code": 200,
                    "content_type": "application/json",
                    "validation_rules": ["Response should contain paginated results according to specified parameters"]
                }
            }
            test_cases.append(pagination_test)
            
            if action.lower() == "search":
                # Search with filters test
                search_test = {
                    "description": f"{action} operation with search filters",
                    "request": {
                        "method": method,
                        "path": path,
                        "headers": {
                            "Accept": "application/json"
                        },
                        "query_params": {
                            "keyword": "test"
                        },
                        "body": None
                    },
                    "expected_response": {
                        "status_code": 200,
                        "content_type": "application/json",
                        "validation_rules": ["Response should contain only resources matching the search criteria"]
                    }
                }
                test_cases.append(search_test)
        
        # Authentication test if specified in requirements
        auth_test = {
            "description": "Unauthorized access attempt",
            "request": {
                "method": method,
                "path": path.replace("{id}", "1"),
                "headers": {
                    "Accept": "application/json"
                },
                "query_params": {},
                "body": None if method == "GET" else self._generate_sample_request_body(entity)
            },
            "expected_response": {
                "status_code": 401,
                "validation_rules": ["Response should indicate authentication error"]
            }
        }
        test_cases.append(auth_test)
        
        return test_cases
    
    def _get_expected_status_code(self, method: str, action: str) -> int:
        """Get the expected HTTP status code for successful operations"""
        if method == "POST":
            return 201
        elif method == "PUT" or method == "DELETE":
            return 204
        else:
            return 200
    
    def _generate_sample_request_body(self, entity: str) -> Dict[str, Any]:
        """Generate a sample request body for the entity"""
        sample_body = {
            "id": 1,
            "name": f"Test {entity}",
            "description": f"This is a test {entity}",
        }
        
        # Add some entity-specific fields
        if entity.lower() == "product":
            sample_body["price"] = 99.99
            sample_body["category"] = "Test Category"
        elif entity.lower() == "order":
            sample_body["customerId"] = 1
            sample_body["orderDate"] = "2023-01-01T00:00:00Z"
            sample_body["status"] = "Pending"
        elif entity.lower() == "user" or entity.lower() == "customer":
            sample_body["email"] = "test@example.com"
            sample_body["phoneNumber"] = "555-1234"
        
        return sample_body
    
    def _generate_playwright_test_code(self, endpoint: Dict[str, Any], test_cases: List[Dict[str, Any]]) -> str:
        """Generate Playwright test code for the API endpoint"""
        method = endpoint.get("method", "GET")
        path = endpoint.get("endpoint", "/api/resource")
        action = endpoint.get("action", "")
        entity = endpoint.get("entity", "resource")
        
        test_code = f"""
using System.Threading.Tasks;
using System.Text.Json;
using Microsoft.Playwright;
using Xunit;
using API.Tests.Fixtures;
using System.Collections.Generic;
using System.Net;

namespace API.Tests
{{
    public class {entity}ApiTests : IClassFixture<TestFixture>
    {{
        private readonly TestFixture _fixture;
        private readonly string _baseUrl;
        private readonly string _apiEndpoint = "{path}";
        
        public {entity}ApiTests(TestFixture fixture)
        {{
            _fixture = fixture;
            _baseUrl = "http://localhost:5000";
        }}
        
"""
        
        # Generate test methods for each test case
        for i, test_case in enumerate(test_cases):
            description = test_case["description"]
            method_name = description.replace(" ", "_").replace("-", "_").replace("(", "").replace(")", "")
            expected_status = test_case["expected_response"]["status_code"]
            
            test_code += f"""        [Fact]
        public async Task {method_name}()
        {{
            // Arrange
"""
            
            # Request setup
            if test_case["request"]["body"]:
                test_code += f"            var requestBody = {json.dumps(test_case['request']['body'], indent=12)};\n"
            
            query_params = test_case["request"]["query_params"]
            if query_params:
                query_string = "&".join([f"{k}={v}" for k, v in query_params.items()])
                request_path = f"{test_case['request']['path']}?{query_string}"
            else:
                request_path = test_case["request"]["path"]
            
            # Action section
            test_code += f"\n            // Act\n"
            
            if method == "GET":
                test_code += f"            var response = await _fixture.Request.GetAsync($\"{{_baseUrl}}{request_path}\");\n"
            elif method == "POST":
                test_code += f"            var response = await _fixture.Request.PostAsync($\"{{_baseUrl}}{request_path}\", new APIRequestContextOptions\n"
                test_code += "            {\n"
                if test_case["request"]["body"]:
                    test_code += "                DataObject = requestBody\n"
                test_code += "            });\n"
            elif method == "PUT":
                test_code += f"            var response = await _fixture.Request.PutAsync($\"{{_baseUrl}}{request_path}\", new APIRequestContextOptions\n"
                test_code += "            {\n"
                if test_case["request"]["body"]:
                    test_code += "                DataObject = requestBody\n"
                test_code += "            });\n"
            elif method == "DELETE":
                test_code += f"            var response = await _fixture.Request.DeleteAsync($\"{{_baseUrl}}{request_path}\");\n"
            
            # Assert section
            test_code += f"\n            // Assert\n"
            test_code += f"            Assert.Equal(HttpStatusCode.{self._status_code_to_name(expected_status)}, (HttpStatusCode)response.Status);\n"
            
            # Content type assertion if specified
            if "content_type" in test_case["expected_response"]:
                test_code += f"            Assert.Contains(\"{test_case['expected_response']['content_type']}\", response.Headers[\"content-type\"]);\n"
            
            # Body content assertions if needed
            if "body_contains" in test_case["expected_response"] and test_case["expected_response"]["body_contains"]:
                test_code += "\n            var responseBody = await response.JsonAsync();\n"
                test_code += "            Assert.NotNull(responseBody);\n"
                
                for field in test_case["expected_response"]["body_contains"]:
                    test_code += f"            Assert.True(responseBody.Value.TryGetProperty(\"{field}\", out _), \"Response should contain {field} property\");\n"
            
            test_code += "        }\n\n"
        
        test_code += "    }\n}"
        
        return test_code
    
    def _status_code_to_name(self, status_code: int) -> str:
        """Convert HTTP status code to its enum name in C#"""
        status_map = {
            200: "OK",
            201: "Created",
            204: "NoContent",
            400: "BadRequest",
            401: "Unauthorized",
            403: "Forbidden",
            404: "NotFound",
            500: "InternalServerError"
        }
        return status_map.get(status_code, f"StatusCode{status_code}")
    
    def _evaluate_test_coverage(self, endpoint: Dict[str, Any], test_cases: List[Dict[str, Any]]) -> Dict[str, Any]:
        """Evaluate the test coverage based on the generated test cases"""
        coverage_evaluation = {
            "happy_path_coverage": False,
            "validation_coverage": False,
            "error_handling_coverage": False,
            "authentication_coverage": False,
            "edge_cases_coverage": False,
            "coverage_score": 0,
            "missing_test_cases": []
        }
        
        # Check for happy path coverage
        if any("Valid" in tc["description"] for tc in test_cases):
            coverage_evaluation["happy_path_coverage"] = True
        else:
            coverage_evaluation["missing_test_cases"].append("Happy path test case")
        
        # Check for validation coverage
        if any("Invalid" in tc["description"] and "missing required fields" in tc["description"] for tc in test_cases):
            coverage_evaluation["validation_coverage"] = True
        else:
            method = endpoint.get("method", "")
            if method in ["POST", "PUT", "PATCH"]:
                coverage_evaluation["missing_test_cases"].append("Input validation test case")
        
        # Check for error handling coverage
        if any("non-existent ID" in tc["description"] for tc in test_cases):
            coverage_evaluation["error_handling_coverage"] = True
        else:
            action = endpoint.get("action", "").lower()
            if action in ["read", "update", "delete"]:
                coverage_evaluation["missing_test_cases"].append("Not found error test case")
        
        # Check for authentication coverage
        if any("Unauthorized" in tc["description"] for tc in test_cases):
            coverage_evaluation["authentication_coverage"] = True
        else:
            coverage_evaluation["missing_test_cases"].append("Authentication test case")
        
        # Check for edge cases coverage
        edge_case_patterns = ["mismatched", "pagination", "search filters"]
        edge_cases_found = any(any(pattern in tc["description"] for pattern in edge_case_patterns) for tc in test_cases)
        if edge_cases_found:
            coverage_evaluation["edge_cases_coverage"] = True
        else:
            coverage_evaluation["missing_test_cases"].append("Edge case test scenarios")
        
        # Calculate coverage score (simple algorithm)
        coverage_aspects = [
            "happy_path_coverage",
            "validation_coverage",
            "error_handling_coverage",
            "authentication_coverage",
            "edge_cases_coverage"
        ]
        
        coverage_count = sum(1 for aspect in coverage_aspects if coverage_evaluation[aspect])
        coverage_evaluation["coverage_score"] = int((coverage_count / len(coverage_aspects)) * 100)
        
        return coverage_evaluation
    
    def _run(self, endpoint: Dict[str, Any]) -> str:
        """
        Generate and evaluate API tests for an endpoint
        """
        # Generate test cases
        test_cases = self._generate_api_test_cases(endpoint)
        
        # Generate Playwright test code
        playwright_test_code = self._generate_playwright_test_code(endpoint, test_cases)
        
        # Evaluate test coverage
        coverage_evaluation = self._evaluate_test_coverage(endpoint, test_cases)
        
        # Prepare the result
        result = {
            "endpoint": endpoint,
            "test_cases": test_cases,
            "playwright_test_code": playwright_test_code,
            "coverage_evaluation": coverage_evaluation
        }
        
        return json.dumps(result, indent=2)

class SecurityAnalysisTool(Tool):
    name = "SecurityAnalyzer"
    description = "Analyzes code for security vulnerabilities and best practices"
    
    def _check_for_sql_injection(self, code: str) -> List[Dict[str, Any]]:
        """Check for potential SQL injection vulnerabilities"""
        vulnerabilities = []
        
        # Check for raw SQL queries with string concatenation
        raw_sql_patterns = [
            r'Execute[Scalar|NonQuery|Reader]\(.*\+',
            r'SqlCommand\(.*\+',
            r'FromSqlRaw\(.*\+',
            r'ExecuteSqlRaw\(.*\+',
        ]
        
        for pattern in raw_sql_patterns:
            matches = re.findall(pattern, code)
            if matches:
                vulnerabilities.append({
                    "type": "sql_injection",
                    "severity": "high",
                    "description": "Potential SQL injection vulnerability: using string concatenation in SQL queries",
                    "remediation": "Use parameterized queries with EF Core methods or SQL parameters",
                    "code_pattern": pattern,
                    "match_count": len(matches)
                })
        
        # Check for proper parameter usage in SQL
        proper_param_patterns = [
            r'Execute\w+\(".*", new \{ .* \}\)',
            r'SqlParameter\(',
            r'FromSqlInterpolated\(',
        ]
        
        proper_params_found = any(re.search(pattern, code) for pattern in proper_param_patterns)
        
        if "SqlCommand" in code and not proper_params_found:
            vulnerabilities.append({
                "type": "sql_injection",
                "severity": "medium",
                "description": "Possible improper SQL parameter usage: SQL commands found without proper parameterization evidence",
                "remediation": "Ensure all SQL queries use parameters via SqlParameter objects or EF Core methods",
                "code_pattern": "SqlCommand without SqlParameter",
                "match_count": 1
            })
        
        return vulnerabilities
    
    def _check_for_xss(self, code: str) -> List[Dict[str, Any]]:
        """Check for potential Cross-Site Scripting (XSS) vulnerabilities"""
        vulnerabilities = []
        
        # Check for unencoded output in Razor views
        if ".cshtml" in code:
            unencoded_patterns = [
                r'@Html\.Raw\(',
                r'@\(',  # Check for @(...) expressions that might need encoding
            ]
            
            for pattern in unencoded_patterns:
                matches = re.findall(pattern, code)
                if matches and pattern == r'@Html\.Raw\(':
                    vulnerabilities.append({
                        "type": "xss",
                        "severity": "high",
                        "description": "Potential XSS vulnerability: using Html.Raw() to output unencoded content",
                        "remediation": "Use encoded outputs like Html.Encode() or @model.Property syntax which auto-encodes",
                        "code_pattern": pattern,
                        "match_count": len(matches)
                    })
        
        # Check for potentially unsafe API responses
        if "Controller" in code:
            # Look for returning raw string content without proper content type
            unsafe_response_patterns = [
                r'return\s+Content\([^,]+\)',  # Missing content type
                r'JavaScriptSerializer',  # Using older serialization methods that might not encode properly
            ]
            
            for pattern in unsafe_response_patterns:
                matches = re.findall(pattern, code)
                if matches:
                    vulnerabilities.append({
                        "type": "xss",
                        "severity": "medium",
                        "description": "Potential XSS in API responses: returning content without proper encoding or content type",
                        "remediation": "Use structured responses (Json) with proper content types",
                        "code_pattern": pattern,
                        "match_count": len(matches)
                    })
        
        return vulnerabilities
    
    def _check_for_csrf(self, code: str) -> List[Dict[str, Any]]:
        """Check for potential Cross-Site Request Forgery (CSRF) vulnerabilities"""
        vulnerabilities = []
        
        # Check for missing anti-forgery tokens in forms or POST actions
        if "Controller" in code and ("HttpPost" in code or "HttpPut" in code or "HttpDelete" in code):
            antiforgery_patterns = [
                r'\[ValidateAntiForgeryToken\]',
                r'@Html\.AntiForgeryToken\(\)',
                r'<input name="__RequestVerificationToken"',
            ]
            
            antiforgery_found = any(re.search(pattern, code) for pattern in antiforgery_patterns)
            
            if not antiforgery_found:
                vulnerabilities.append({
                    "type": "csrf",
                    "severity": "medium",
                    "description": "Potential CSRF vulnerability: missing anti-forgery token validation on state-changing operations",
                    "remediation": "Add [ValidateAntiForgeryToken] attribute to controllers and @Html.AntiForgeryToken() to forms",
                    "code_pattern": "POST/PUT                    story_title = user_story.get("title", "Unnamed User Story")
                    all_questions.append({
                        "section": f"User Story: {story_id} - {story_title}",
                        "questions": questions
                    })
        
        # Technical requirements questions
        if "technical_requirements" in requirements:
            for tech_req in requirements["technical_requirements"]:
                questions = self._generate_questions_for_technical_requirement(tech_req)
                if questions:
                    req_id = tech_req.get("id", "")
                    req_title = tech_req.get("title", "Unnamed Technical Requirement")
                    all_questions.append({
                        "section": f"Technical Requirement: {req_id} - {req_title}",
                        "questions": questions
                    })
        
        # Non-functional requirements questions
        if "non_functional_requirements" in requirements:
            for nfr in requirements["non_functional_requirements"]:
                questions = self._generate_questions_for_non_functional_requirement(nfr)
                if questions:
                    nfr_id = nfr.get("id", "")
                    nfr_title = nfr.get("title", "Unnamed Non-Functional Requirement")
                    all_questions.append({
                        "section": f"Non-Functional Requirement: {nfr_id} - {nfr_title}",
                        "questions": questions
                    })
        
        # Format the questions as a markdown document
        markdown = "# Requirements Clarification Questions\n\n"
        
        for section in all_questions:
            markdown += f"## {section['section']}\n\n"
            for i, question in enumerate(section['questions'], 1):
                markdown += f"{i}. {question}\n"
            markdown += "\n"
        
        return markdown

class RequirementTransformationTool(Tool):
    name = "RequirementTransformer"
    description = "Transforms raw requirements and answers into structured specifications for development"
    
    def _structure_user_story(self, user_story: Dict[str, Any], answers: Dict[str, str]) -> Dict[str, Any]:
        """Enhance a user story with answers to clarification questions"""
        
        enhanced_story = user_story.copy()
        
        # Extract story ID for matching answers
        story_id = user_story.get("id", "")
        story_title = user_story.get("title", "")
        story_identifier = f"{story_id} - {story_title}" if story_id else story_title
        
        # Find answers relevant to this user story
        relevant_answers = {}
        for question, answer in answers.items():
            if story_identifier in question:
                relevant_answers[question] = answer
        
        # Add or enhance fields based on answers
        if "acceptance_criteria" not in enhanced_story or not enhanced_story["acceptance_criteria"]:
            enhanced_story["acceptance_criteria"] = []
            
        for question, answer in relevant_answers.items():
            # Extract criteria from answers
            if "acceptance criteria" in question.lower():
                # Split answer into individual criteria
                new_criteria = [c.strip() for c in answer.split('\n') if c.strip()]
                for criterion in new_criteria:
                    if criterion not in enhanced_story["acceptance_criteria"]:
                        enhanced_story["acceptance_criteria"].append(criterion)
            
            # Extract data fields
            elif "data fields" in question.lower() or "fields" in question.lower():
                if "data_fields" not in enhanced_story:
                    enhanced_story["data_fields"] = []
                new_fields = [f.strip() for f in answer.split(',') if f.strip()]
                enhanced_story["data_fields"].extend(new_fields)
            
            # Extract validation rules
            elif "validation" in question.lower():
                if "validation_rules" not in enhanced_story:
                    enhanced_story["validation_rules"] = []
                new_rules = [r.strip() for r in answer.split('\n') if r.strip()]
                enhanced_story["validation_rules"].extend(new_rules)
            
            # Extract search/filter criteria
            elif "search" in question.lower() or "filter" in question.lower():
                if "search_criteria" not in enhanced_story:
                    enhanced_story["search_criteria"] = []
                new_criteria = [c.strip() for c in answer.split(',') if c.strip()]
                enhanced_story["search_criteria"].extend(new_criteria)
            
            # Extract pagination info
            elif "pagination" in question.lower():
                enhanced_story["pagination"] = answer.strip()
            
            # Extract edge cases
            elif "edge cases" in question.lower():
                if "edge_cases" not in enhanced_story:
                    enhanced_story["edge_cases"] = []
                new_cases = [c.strip() for c in answer.split('\n') if c.strip()]
                enhanced_story["edge_cases"].extend(new_cases)
            
            # Extract performance requirements
            elif "performance" in question.lower():
                if "performance_requirements" not in enhanced_story:
                    enhanced_story["performance_requirements"] = []
                new_requirements = [r.strip() for r in answer.split('\n') if r.strip()]
                enhanced_story["performance_requirements"].extend(new_requirements)
        
        return enhanced_story
    
    def _structure_technical_requirement(self, tech_req: Dict[str, Any], answers: Dict[str, str]) -> Dict[str, Any]:
        """Enhance a technical requirement with answers to clarification questions"""
        
        enhanced_req = tech_req.copy()
        
        # Extract req ID for matching answers
        req_id = tech_req.get("id", "")
        req_title = tech_req.get("title", "")
        req_identifier = f"{req_id} - {req_title}" if req_id else req_title
        
        # Find answers relevant to this requirement
        relevant_answers = {}
        for question, answer in answers.items():
            if req_identifier in question:
                relevant_answers[question] = answer
        
        # Add or enhance fields based on answers
        if "details" not in enhanced_req or not enhanced_req["details"]:
            enhanced_req["details"] = []
            
        for question, answer in relevant_answers.items():
            # Extract details from answers
            if "specific" in question.lower() and "details" in question.lower():
                # Split answer into individual details
                new_details = [d.strip() for d in answer.split('\n') if d.strip()]
                for detail in new_details:
                    if detail not in enhanced_req["details"]:
                        enhanced_req["details"].append(detail)
            
            # Database specific answers
            elif any(word in question.lower() for word in ["database", "indexing", "data volume"]):
                if "database_details" not in enhanced_req:
                    enhanced_req["database_details"] = []
                new_details = [d.strip() for d in answer.split('\n') if d.strip()]
                enhanced_req["database_details"].extend(new_details)
            
            # Performance specific answers
            elif "performance" in question.lower():
                if "performance_details" not in enhanced_req:
                    enhanced_req["performance_details"] = []
                new_details = [d.strip() for d in answer.split('\n') if d.strip()]
                enhanced_req["performance_details"].extend(new_details)
            
            # Security specific answers
            elif "security" in question.lower():
                if "security_details" not in enhanced_req:
                    enhanced_req["security_details"] = []
                new_details = [d.strip() for d in answer.split('\n') if d.strip()]
                enhanced_req["security_details"].extend(new_details)
            
            # Integration specific answers
            elif "integration" in question.lower():
                if "integration_details" not in enhanced_req:
                    enhanced_req["integration_details"] = []
                new_details = [d.strip() for d in answer.split('\n') if d.strip()]
                enhanced_req["integration_details"].extend(new_details)
        
        return enhanced_req
    
    def _structure_non_functional_requirement(self, nfr: Dict[str, Any], answers: Dict[str, str]) -> Dict[str, Any]:
        """Enhance a non-functional requirement with answers to clarification questions"""
        
        enhanced_nfr = nfr.copy()
        
        # Extract nfr ID for matching answers
        nfr_id = nfr.get("id", "")
        nfr_title = nfr.get("title", "")
        nfr_identifier = f"{nfr_id} - {nfr_title}" if nfr_id else nfr_title
        
        # Find answers relevant to this NFR
        relevant_answers = {}
        for question, answer in answers.items():
            if nfr_identifier in question:
                relevant_answers[question] = answer
        
        # Add or enhance fields based on answers
        if "details" not in enhanced_nfr or not enhanced_nfr["details"]:
            enhanced_nfr["details"] = []
            
        for question, answer in relevant_answers.items():
            # Extract details from answers
            if "specific" in question.lower() and "details" in question.lower():
                # Split answer into individual details
                new_details = [d.strip() for d in answer.split('\n') if d.strip()]
                for detail in new_details:
                    if detail not in enhanced_nfr["details"]:
                        enhanced_nfr["details"].append(detail)
            
            # Specific metric answers
            elif "metric" in question.lower() or "measure" in question.lower():
                if "metrics" not in enhanced_nfr:
                    enhanced_nfr["metrics"] = []
                new_metrics = [m.strip() for m in answer.split('\n') if m.strip()]
                enhanced_nfr["metrics"].extend(new_metrics)
        
        return enhanced_nfr
    
    def _enhance_database_requirements(self, db_reqs: Dict[str, Any], answers: Dict[str, str]) -> Dict[str, Any]:
        """Enhance database requirements with general answers"""
        
        enhanced_db_reqs = db_reqs.copy() if db_reqs else {}
        
        # Check for database choice answer
        for question, answer in answers.items():
            if "database" in question.lower() and "primary" in question.lower():
                enhanced_db_reqs["primary_database"] = answer.strip()
        
        return enhanced_db_reqs
    
    def _enhance_general_requirements(self, requirements: Dict[str, Any], answers: Dict[str, str]) -> Dict[str, Any]:
        """Enhance general requirements with answers"""
        
        enhanced_reqs = requirements.copy()
        
        # Add missing sections if answers provide them
        if "database_requirements" not in enhanced_reqs:
            enhanced_reqs["database_requirements"] = {}
        
        for question, answer in answers.items():
            if "General Requirements" in question:
                if "database engine" in question.lower() and answer:
                    enhanced_reqs["database_requirements"]["primary_database"] = answer.strip()
                
                elif "architectural patterns" in question.lower() and answer:
                    if "architectural_requirements" not in enhanced_reqs:
                        enhanced_reqs["architectural_requirements"] = {}
                    enhanced_reqs["architectural_requirements"]["patterns"] = [p.strip() for p in answer.split('\n') if p.strip()]
                
                elif "existing systems" in question.lower() and "integrate" in question.lower() and answer:
                    if "integration_requirements" not in enhanced_reqs:
                        enhanced_reqs["integration_requirements"] = []
                    systems = [s.strip() for s in answer.split(',') if s.strip()]
                    for system in systems:
                        enhanced_reqs["integration_requirements"].append({
                            "title": f"Integration with {system}",
                            "description": f"API must integrate with {system}.",
                            "details": []
                        })
                
                elif "authentication" in question.lower() and "authorization" in question.lower() and answer:
                    if "security_requirements" not in enhanced_reqs:
                        enhanced_reqs["security_requirements"] = {}
                    enhanced_reqs["security_requirements"]["authentication"] = answer.strip()
                
                elif "performance requirements" in question.lower() and answer:
                    if "performance_requirements" not in enhanced_reqs:
                        enhanced_reqs["performance_requirements"] = {}
                    enhanced_reqs["performance_requirements"]["general"] = answer.strip()
                
                elif "deployment" in question.lower() and answer:
                    if "deployment_requirements" not in enhanced_reqs:
                        enhanced_reqs["deployment_requirements"] = {}
                    enhanced_reqs["deployment_requirements"]["environment"] = answer.strip()
        
        return enhanced_reqs
    
    def _run(self, requirements: Dict[str, Any], answers: Dict[str, str]) -> str:
        """Transform requirements using answers to clarification questions"""
        
        enhanced_requirements = requirements.copy()
        
        # Enhance general requirements
        enhanced_requirements = self._enhance_general_requirements(enhanced_requirements, answers)
        
        # Enhance database requirements if they exist
        if "database_requirements" in enhanced_requirements:
            enhanced_requirements["database_requirements"] = self._enhance_database_requirements(
                enhanced_requirements["database_requirements"], 
                answers
            )
        
        # Enhance user stories if they exist
        if "user_stories" in enhanced_requirements:
            enhanced_stories = []
            for story in enhanced_requirements["user_stories"]:
                enhanced_stories.append(self._structure_user_story(story, answers))
            enhanced_requirements["user_stories"] = enhanced_stories
        
        # Enhance technical requirements if they exist
        if "technical_requirements" in enhanced_requirements:
            enhanced_tech_reqs = []
            for req in enhanced_requirements["technical_requirements"]:
                enhanced_tech_reqs.append(self._structure_technical_requirement(req, answers))
            enhanced_requirements["technical_requirements"] = enhanced_tech_reqs
        
        # Enhance non-functional requirements if they exist
        if "non_functional_requirements" in enhanced_requirements:
            enhanced_nfrs = []
            for nfr in enhanced_requirements["non_functional_requirements"]:
                enhanced_nfrs.append(self._structure_non_functional_requirement(nfr, answers))
            enhanced_requirements["non_functional_requirements"] = enhanced_nfrs
        
        return json.dumps(enhanced_requirements, indent=2)

class AcceptanceCriteriaValidationTool(Tool):
    name = "AcceptanceCriteriaValidator"
    description = "Validates implementations against specified acceptance criteria"
    
    def _parse_acceptance_criteria(self, acceptance_criteria: List[str]) -> List[Dict[str, Any]]:
        """Parse acceptance criteria into structured format for validation"""
        structured_criteria = []
        
        for criterion in acceptance_criteria:
            criterion = criterion.strip()
            structured_criterion = {
                "original_text": criterion,
                "type": "unknown",
                "validation_method": "manual",
                "automated_test_possible": False
            }
            
            # Determine criterion type and possible validation method
            if "must" in criterion.lower() or "should" in criterion.lower() or "shall" in criterion.lower():
                structured_criterion["type"] = "requirement"
                
                # Check if it's a functional requirement that could be tested
                if any(word in criterion.lower() for word in ["return", "display", "show", "create", "update", "delete", "validate"]):
                    structured_criterion["validation_method"] = "functional_test"
                    structured_criterion["automated_test_possible"] = True
            
            elif "validate" in criterion.lower() or "verify" in criterion.lower() or "check" in criterion.lower():
                structured_criterion["type"] = "validation"
                structured_criterion["validation_method"] = "functional_test"
                structured_criterion["automated_test_possible"] = True
            
            # Performance-related criteria
            elif any(word in criterion.lower() for word in ["performance", "response time", "throughput", "load"]):
                structured_criterion["type"] = "performance"
                structured_criterion["validation_method"] = "performance_test"
                structured_criterion["automated_test_possible"] = True
            
            # Security-related criteria
            elif any(word in criterion.lower() for word in ["secure", "security", "authorize", "authenticate", "permission"]):
                structured_criterion["type"] = "security"
                structured_criterion["validation_method"] = "security_test"
                structured_criterion["automated_test_possible"] = True
            
            structured_criteria.append(structured_criterion)
        
        return structured_criteria
    
    def _validate_implementation(self, implementation: str, structured_criteria: List[Dict[str, Any]]) -> List[Dict[str, Any]]:
        """Validate implementation against structured acceptance criteria"""
        validation_results = []
        
        for criterion in structured_criteria:
            result = {
                "criterion": criterion["original_text"],
                "validation_status": "inconclusive",
                "evidence": "",
                "recommendations": []
            }
            
            # Basic text-based validation - check if implementation mentions key concepts
            criterion_keywords = set(criterion["original_text"].lower().split())
            # Remove common words
            criterion_keywords = criterion_keywords - set(["the", "a", "an", "and", "or", "must", "should", "shall", "be", "to", "in", "for", "with", "that", "is", "are", "on", "of"])
            
            # Count how many keywords from the criterion are found in the implementation
            found_keywords = [keyword for keyword in criterion_keywords if keyword.lower() in implementation.lower()]
            keyword_coverage = len(found_keywords) / len(criterion_keywords) if criterion_keywords else 0
            
            if keyword_coverage > 0.7:
                result["validation_status"] = "likely_implemented"
                result["evidence"] = f"Implementation contains {len(found_keywords)}/{len(criterion_keywords)} key concepts from the criterion"
            elif 0.4 <= keyword_coverage <= 0.7:
                result["validation_status"] = "partially_implemented"
                result["evidence"] = f"Implementation contains some ({len(found_keywords)}/{len(criterion_keywords)}) key concepts, but may not fully satisfy the criterion"
                result["recommendations"].append("Add explicit handling for all aspects of this criterion")
            else:
                result["validation_status"] = "not_implemented"
                result["evidence"] = f"Implementation contains few ({len(found_keywords)}/{len(criterion_keywords)}) key concepts from the criterion"
                result["recommendations"].append("Implement handling for this criterion")
            
            # Additional validation based on criterion type
            if criterion["type"] == "validation" and "validation" not in implementation.lower() and "validate" not in implementation.lower():
                result["recommendations"].append("Add explicit validation logic")
            
            elif criterion["type"] == "performance" and not any(term in implementation.lower() for term in ["cach", "optimi", "async", "parallel"]):
                result["recommendations"].append("Consider adding performance optimizations")
            
            elif criterion["type"] == "security" and not any(term in implementation.lower() for term in ["authori", "authenti", "permission", "role", "claim"]):
                result["recommendations"].append("Add explicit security controls")
            
            validation_results.append(result)
        
        return validation_results
    
    def _generate_test_suggestions(self, structured_criteria: List[Dict[str, Any]]) -> List[Dict[str, str]]:
        """Generate test suggestions for each acceptance criterion"""
        test_suggestions = []
        
        for criterion in structured_criteria:
            if criterion["automated_test_possible"]:
                if criterion["validation_method"] == "functional_test":
                    criterion_text = criterion["original_text"].lower()
                    
                    if "create" in criterion_text:
                        test_suggestions.append({
                            "criterion": criterion["original_text"],
                            "test_type": "API Test",
                            "test_description": f"Test that performs a POST request and verifies the resource is created correctly, validating all required fields and business rules described in '{criterion['original_text']}'"
                        })
                    elif "update" in criterion_text:
                        test_suggestions.append({
                            "criterion": criterion["original_text"],
                            "test_type": "API Test",
                            "test_description": f"Test that performs a PUT/PATCH request and verifies the resource is updated correctly, validating all fields and business rules described in '{criterion['original_text']}'"
                        })
                    elif "delete" in criterion_text:
                        test_suggestions.append({
                            "criterion": criterion["original_text"],
                            "test_type": "API Test",
                            "test_description": f"Test that performs a DELETE request and verifies the resource is deleted correctly, validating any business rules described in '{criterion['original_text']}'"
                        })
                    elif any(word in criterion_text for word in ["get", "retrieve", "list", "search", "filter"]):
                        test_suggestions.append({
                            "criterion": criterion["original_text"],
                            "test_type": "API Test",
                            "test_description": f"Test that performs a GET request and verifies the response contains the correct data, validating any filtering, sorting, or pagination rules described in '{criterion['original_text']}'"
                        })
                    elif "validate" in criterion_text or "validation" in criterion_text:
                        test_suggestions.append({
                            "criterion": criterion["original_text"],
                            "test_type": "Validation Test",
                            "test_description": f"Test that verifies the validation rules described in '{criterion['original_text']}', including both positive cases (valid data) and negative cases (invalid data)"
                        })
                
                elif criterion["validation_method"] == "performance_test":
                    test_suggestions.append({
                        "criterion": criterion["original_text"],
                        "test_type": "Performance Test",
                        "test_description": f"Test that measures response time, throughput, or resource usage under various load conditions to verify the performance requirements described in '{criterion['original_text']}'"
                    })
                
                elif criterion["validation_method"] == "security_test":
                    test_suggestions.append({
                        "criterion": criterion["original_text"],
                        "test_type": "Security Test",
                        "test_description": f"Test that validates the security controls, authentication, authorization, or data protection mechanisms described in '{criterion['original_text']}'"
                    })
        
        return test_suggestions
    
    def _run(self, user_story: Dict[str, Any], implementation: str) -> str:
        """
        Validate implementation against the acceptance criteria of a user story
        and provide test suggestions
        """
        # Extract acceptance criteria from user story
        acceptance_criteria = user_story.get("acceptance_criteria", [])
        
        if not acceptance_criteria:
            return json.dumps({
                "validation_status": "no_criteria",
                "message": "No acceptance criteria found in the user story",
                "validation_results": [],
                "test_suggestions": []
            }, indent=2)
        
        # Parse acceptance criteria into structured format
        structured_criteria = self._parse_acceptance_criteria(acceptance_criteria)
        
        # Validate implementation against structured criteria
        validation_results = self._validate_implementation(implementation, structured_criteria)
        
        # Generate test suggestions
        test_suggestions = self._generate_test_suggestions(structured_criteria)
        
        # Determine overall validation status
        status_counts = {
            "likely_implemented": sum(1 for result in validation_results if result["validation_status"] == "likely_implemented"),
            "partially_implemented": sum(1 for result in validation_results if result["validation_status"] == "partially_implemented"),
            "not_implemented": sum(1 for result in validation_results if result["validation_status"] == "not_implemented")
        }
        
        if status_counts["not_implemented"] > 0:
            overall_status = "incomplete"
        elif status_counts["partially_implemented"] > 0:
            overall_status = "needs_improvement"
        else:
            overall_status = "satisfactory"
        
        # Return results as JSON
        return json.dumps({
            "validation_status": overall_status,
            "summary": f"{status_counts['likely_implemented']} criteria likely implemented, {status_counts['partially_implemented']} partially implemented, {status_counts['not_implemented']} not implemented",
            "validation_results": validation_results,
            "test_suggestions": test_suggestions
        }, indent=2)

class CodeQualityAnalysisTool(Tool):
    name = "CodeQualityAnalyzer"
    description = "Analyzes code quality against best practices and coding standards"
    
    def _analyze_code_complexity(self, code: str) -> Dict[str, Any]:
        """Analyze code complexity metrics"""
        analysis = {
            "cyclomatic_complexity": 0,
            "nesting_depth": 0,
            "method_length": 0,
            "class_length": 0
        }
        
        # Count control flow statements as a simple proxy for cyclomatic complexity
        control_flow_keywords = ["if", "else", "for", "foreach", "while", "switch", "case", "catch", "return"]
        for keyword in control_flow_keywords:
            analysis["cyclomatic_complexity"] += len(re.findall(r'\b' + keyword + r'\b', code))
        
        # Estimate maximum nesting depth
        lines = code.split('\n')
        current_depth = 0
        max_depth = 0
        for line in lines:
            line = line.strip()
            # Count opening braces that are not part of an initialization
            if '{' in line and not ('=' in line and '{' in line.split('=')[1]):
                current_depth += 1
                max_depth = max(max_depth, current_depth)
            if '}' in line:
                current_depth = max(0, current_depth - 1)
        analysis["nesting_depth"] = max_depth
        
        # Estimate method length by finding method blocks
        method_pattern = r'(public|private|protected|internal)\s+(async\s+)?([\w<>[\],\s]+)\s+(\w+)\s*\([^)]*\)\s*\{[^}]*\}'
        methods = re.findall(method_pattern, code, re.DOTALL)
        if methods:
            max_method_length = max(len(method[0].split('\n')) for method in methods)
            analysis["method_length"] = max_method_length
        
        # Estimate class length
        class_pattern = r'class\s+\w+[^{]*\{[^}]*\}'
        classes = re.findall(class_pattern, code, re.DOTALL)
        if classes:
            max_class_length = max(len(class_match.split('\n')) for class_match in classes)
            analysis["class_length"] = max_class_length
        
        return analysis
    
    def _check_naming_conventions(self, code: str) -> Dict[str, List[str]]:
        """Check adherence to .NET naming conventions"""
        issues = {
            "classes": [],
            "methods": [],
            "parameters": [],
            "variables": [],
            "properties": []
        }
        
        # Check class names (PascalCase)
        class_pattern = r'class\s+(\w+)'
        for class_name in re.findall(class_pattern, code):
            if not (class_name[0].isupper() and not class_name.isupper() and '_' not in class_name):
                issues["classes"].append(f"Class '{class_name}' should use PascalCase")
        
        # Check method names (PascalCase)
        method_pattern = r'(public|private|protected|internal)\s+(async\s+)?([\w<>[\],\s]+)\s+(\w+)\s*\('
        for match in re.finditer(method_pattern, code):
            method_name = match.group(4)
            if not (method_name[0].isupper() and not method_name.isupper() and '_' not in method_name):
                issues["methods"].append(f"Method '{method_name}' should use PascalCase")
        
        # Check parameter names (camelCase)
        param_pattern = r'\(([^)]*)\)'
        for params_str in re.findall(param_pattern, code):
            if params_str.strip():
                params = [p.strip() for p in params_str.split(',')]
                for param in params:
                    parts = param.split()
                    if len(parts) >= 2:
                        param_name = parts[-1]
                        if param_name.startswith('@'):  # Handle C# verbatim identifiers
                            param_name = param_name[1:]
                        if not (param_name[0].islower() and '_' not in param_name):
                            issues["parameters"].append(f"Parameter '{param_name}' should use camelCase")
        
        # Check local variable names (camelCase)
        var_pattern = r'var\s+(\w+)\s*='
        for var_name in re.findall(var_pattern, code):
            if not (var_name[0].islower() and '_' not in var_name):
                issues["variables"].append(f"Variable '{var_name}' should use camelCase")
        
        # Check property names (PascalCase)
        prop_pattern = r'(public|private|protected|internal)\s+([\w<>[\],\s]+)\s+(\w+)\s*{\s*get;'
        for match in re.finditer(prop_pattern, code):
            prop_name = match.group(3)
            if not (prop_name[0].isupper() and not prop_name.isupper() and '_' not in prop_name):
                issues["properties"].append(f"Property '{prop_name}' should use PascalCase")
        
        return issues
    
    def _check_code_smells(self, code: str) -> List[Dict[str, str]]:
        """Identify potential code smells"""
        smells = []
        
        # Check for magic numbers
        numeric_literals = re.findall(r'[^"\w](\d+)[^"\w]', code)
        excluded_values = set(['0', '1', '100', '1000'])  # Common values that might not be magic numbers
        magic_numbers = [num for num in numeric_literals if num not in excluded_values]
        if magic_numbers:
            unique_magic_numbers = set(magic_numbers)
            if len(unique_magic_numbers) > 3:  # More than 3 unique magic numbers might indicate an issue
                smells.append({
                    "type": "magic_numbers",
                    "description": f"Found {len(unique_magic_numbers)} potential magic numbers. Consider using named constants.",
                    "severity": "medium"
                })
        
        # Check for long methods
        method_pattern = r'(public|private|protected|internal)\s+(async\s+)?([\w<>[\],\s]+)\s+(\w+)\s*\([^)]*\)\s*\{([^}]*)\}'
        methods = re.findall(method_pattern, code, re.DOTALL)
        for method_match in methods:
            method_body = method_match[4]
            lines = method_body.count('\n')
            if lines > 30:
                smells.append({
                    "type": "long_method",
                    "description": f"Method '{method_match[3]}' has {lines} lines. Consider breaking it down into smaller methods.",
                    "severity": "high"
                })
            elif lines > 15:        [Fact]
        public async Task Search{entity_name}s_ReturnsFilteredList()
        {{
            // Arrange - Create test items with similar names
            var items = new[] 
            {{
                new {{ name = "Test {entity_name} ABC", description = "Test description 1" }},
                new {{ name = "Test {entity_name} XYZ", description = "Test description 2" }}
            }};
            
            foreach (var item in items)
            {{
                await _fixture.Request.PostAsync(_apiEndpoint, new APIRequestContextOptions
                {{
                    DataObject = item
                }});
            }}
            
            // Act - Search for specific keyword
            var response = await _fixture.Request.GetAsync($"{{_apiEndpoint}}/search?keyword=ABC");
            
            // Assert
            Assert.Equal(HttpStatusCode.OK, (HttpStatusCode)response.Status);
            var responseBody = await response.JsonAsync();
            Assert.NotNull(responseBody);
            Assert.True(responseBody.Value.ValueKind == JsonValueKind.Array);
            
            // Check that results contain only the items with the keyword
            var results = responseBody.Value.EnumerateArray();
            foreach (var result in results)
            {{
                Assert.Contains("ABC", result.GetProperty("name").GetString());
            }}
        }}""")
        
        return "\n\n".join(methods)
    
    def _run(self, entity_name: str, endpoints: List[Dict[str, str]]) -> str:
        generated_tests = {
            "test_fixture": self._generate_test_fixture(entity_name),
            "api_test_class": self._generate_api_test_class(entity_name, endpoints)
        }
        
        return json.dumps(generated_tests, indent=2)

class PerformanceAnalyzerTool(Tool):
    name = "PerformanceAnalyzer"
    description = "Analyzes and optimizes code for performance, with database-specific optimizations"
    
    def _analyze_query(self, query: str, db_engine: str) -> Dict[str, Any]:
        analysis = {
            "query": query,
            "db_engine": db_engine,
            "issues": [],
            "optimizations": []
        }
        
        # Check for common EF Core performance issues
        if "Include" in query and "ThenInclude" in query and ".ToList()" in query:
            analysis["issues"].append({
                "type": "eager_loading",
                "description": "Multiple eager loading operations may cause cartesian explosion",
                "severity": "high"
            })
            
            if db_engine.lower() == "sqlserver":
                analysis["optimizations"].append({
                    "description": "Use AsSplitQuery() to prevent cartesian explosion",
                    "code": query.replace(".Include", ".AsSplitQuery().Include")
                })
            elif db_engine.lower() == "oracle":
                analysis["optimizations"].append({
                    "description": "Use AsSplitQuery() to prevent cartesian explosion with Oracle",
                    "code": query.replace(".Include", ".AsSplitQuery().Include")
                })
        
        if ".Where" in query and not ".AsNoTracking()" in query and ".ToList()" in query:
            analysis["issues"].append({
                "type": "unnecessary_tracking",
                "description": "Query is tracking entities unnecessarily for read-only operation",
                "severity": "medium"
            })
            
            analysis["optimizations"].append({
                "description": "Use AsNoTracking() for read-only queries",
                "code": query.replace(".Where", ".AsNoTracking().Where")
            })
        
        if ".OrderBy" in query and ".Skip" in query and ".Take" in query:
            if db_engine.lower() == "sqlserver":
                analysis["optimizations"].append({
                    "description": "Add index to improve SQL Server paging performance",
                    "code": f"// Add index to the OrderBy column\n// CREATE NONCLUSTERED INDEX IX_OrderByColumn ON TableName(OrderByColumn);"
                })
            elif db_engine.lower() == "oracle":
                analysis["optimizations"].append({
                    "description": "Configure optimal fetch size for Oracle paging",
                    "code": "// In DbContext OnConfiguring:\noptions.UseOracle(connectionString, options => options.MaxBatchSize(100));"
                })
        
        # Database-specific optimizations
        if db_engine.lower() == "sqlserver":
            if not ".TagWith" in query and (".Where" in query or ".OrderBy" in query):
                analysis["optimizations"].append({
                    "description": "Tag queries for better profiling in SQL Server",
                    "code": query.replace("_context", "_context.TagWith(\"QueryName\")")
                })
        elif db_engine.lower() == "oracle":
            if ".Contains" in query:
                analysis["issues"].append({
                    "type": "inefficient_contains",
                    "description": "String Contains() may not use Oracle indexes efficiently",
                    "severity": "medium"
                })
                
                analysis["optimizations"].append({
                    "description": "Use EF.Functions.Like for better Oracle performance with indexing",
                    "code": query.replace(".Contains(", ".StartsWith(") + "\n// Or use: EF.Functions.Like(e.Property, $\"%{keyword}%\")"
                })
        
        return analysis
    
    def _analyze_db_context(self, context_code: str, db_engine: str) -> Dict[str, Any]:
        analysis = {
            "db_engine": db_engine,
            "issues": [],
            "optimizations": []
        }
        
        # Check for common DbContext configuration issues
        if not "optionsBuilder.UseLoggerFactory" in context_code:
            analysis["issues"].append({
                "type": "missing_logging",
                "description": "No query logging configured for performance troubleshooting",
                "severity": "low"
            })
            
            analysis["optimizations"].append({
                "description": "Add logger factory configuration for query logging",
                "code": """// In OnConfiguring:
optionsBuilder.UseLoggerFactory(LoggerFactory.Create(builder => builder.AddConsole()));"""
            })
        
        if not "optionsBuilder.EnableSensitiveDataLogging" in context_code and not "EnableSensitiveDataLogging" in context_code:
            analysis["optimizations"].append({
                "description": "Enable sensitive data logging for development environments",
                "code": """// In OnConfiguring for development only:
if (_environment.IsDevelopment())
{
    optionsBuilder.EnableSensitiveDataLogging();
}"""
            })
        
        # Database-specific optimizations
        if db_engine.lower() == "sqlserver":
            if not "optionsBuilder.UseSqlServer" in context_code or not "CommandTimeout" in context_code:
                analysis["optimizations"].append({
                    "description": "Configure SQL Server with appropriate command timeout",
                    "code": """optionsBuilder.UseSqlServer(
    connectionString,
    options => {
        options.CommandTimeout(30);
        options.EnableRetryOnFailure(3);
    });"""
                })
        elif db_engine.lower() == "oracle":
            if not "options.MaxBatchSize" in context_code:
                analysis["optimizations"].append({
                    "description": "Configure optimal batch size for Oracle",
                    "code": """optionsBuilder.UseOracle(
    connectionString,
    options => {
        options.MaxBatchSize(100);
        options.UseOracleSQLCompatibility("12");
    });"""
                })
        
        return analysis
    
    def _generate_optimization_recommendations(self, analysis: Dict[str, Any]) -> str:
        db_engine = analysis.get("db_engine", "sqlserver")
        recommendations = f"# Performance Optimization Recommendations for {db_engine.upper()}\n\n"
        
        if "issues" in analysis and analysis["issues"]:
            recommendations += "## Identified Issues\n\n"
            for issue in analysis["issues"]:
                recommendations += f"### {issue['type']} ({issue['severity']} severity)\n\n"
                recommendations += f"{issue['description']}\n\n"
        
        if "optimizations" in analysis and analysis["optimizations"]:
            recommendations += "## Recommended Optimizations\n\n"
            for i, opt in enumerate(analysis["optimizations"], 1):
                recommendations += f"### Optimization {i}: {opt['description']}\n\n"
                recommendations += f"```csharp\n{opt['code']}\n```\n\n"
        
        # Add database-specific general recommendations
        recommendations += "## General Recommendations\n\n"
        
        if db_engine.lower() == "sqlserver":
            recommendations += """- Use appropriate SQL Server indexing strategies for frequently queried columns
- Apply AsSplitQuery() for complex includes to prevent cartesian explosion
- Use compiled queries for frequently executed database operations
- Configure appropriate transaction isolation levels
- Apply strategic AsNoTracking() for read-only queries
- Consider SQL Server-specific features like table hints where appropriate
- Add query tags for easier profiling with SQL Server Profiler
- Configure command timeout settings appropriate for your operations
"""
        elif db_engine.lower() == "oracle":
            recommendations += """- Configure Oracle-specific connection settings in DbContext
- Use function-based indexes for complex filtering conditions
- Implement Oracle-specific batch processing for bulk operations
- Apply appropriate FetchSize configuration for large result sets
- Use EF.Functions.Like() instead of string Contains() for better index usage
- Use bind variables correctly to prevent hard parsing
- Configure statement caching appropriately
- Set appropriate MaxBatchSize for optimal performance
"""
        
        return recommendations
    
    def _run(self, code: str, db_engine: str = "sqlserver") -> str:
        if "DbContext" in code:
            analysis = self._analyze_db_context(code, db_engine)
        elif "Include" in code or "Where" in code or "OrderBy" in code:
            analysis = self._analyze_query(code, db_engine)
        else:
            analysis = {
                "db_engine": db_engine,
                "issues": [],
                "optimizations": []
            }
        
        recommendations = self._generate_optimization_recommendations(analysis)
        return recommendations

class DocumentationGeneratorTool(Tool):
    name = "DocumentationGenerator"
    description = "Generates comprehensive API documentation and developer guides"
    
    def _generate_api_documentation(self, endpoints: List[Dict[str, Any]]) -> str:
        doc = "# API Documentation\n\n"
        doc += "## Endpoints\n\n"
        
        for endpoint in endpoints:
            method = endpoint.get("method", "GET")
            path = endpoint.get("endpoint", "/api/resource")
            action = endpoint.get("action", "")
            entity = endpoint.get("entity", "resource")
            
            doc += f"### {method} {path}\n\n"
            
            if action.lower() == "create":
                doc += f"Creates a new {entity}.\n\n"
                doc += "**Request Body:**\n\n"
                doc += "```json\n"
                doc += "{\n"
                doc += f'  "name": "string",\n'
                doc += f'  "description": "string"\n'
                doc += "}\n"
                doc += "```\n\n"
                doc += "**Response:**\n\n"
                doc += "- Status: 201 Created\n"
                doc += "- Content:\n\n"
                doc += "```json\n"
                doc += "{\n"
                doc += f'  "id": 1,\n'
                doc += f'  "name": "string",\n'
                doc += f'  "description": "string",\n'
                doc += f'  "createdAt": "2023-01-01T00:00:00Z",\n'
                doc += f'  "updatedAt": null\n'
                doc += "}\n"
                doc += "```\n\n"
            elif action.lower() == "read" or action.lower() == "get":
                doc += f"Retrieves a specific {entity} by ID.\n\n"
                doc += "**Parameters:**\n\n"
                doc += "- `id` (path parameter): The ID of the resource\n\n"
                doc += "**Response:**\n\n"
                doc += "- Status: 200 OK\n"
                doc += "- Content:\n\n"
                doc += "```json\n"
                doc += "{\n"
                doc += f'  "id": 1,\n'
                doc += f'  "name": "string",\n'
                doc += f'  "description": "string",\n'
                doc += f'  "createdAt": "2023-01-01T00:00:00Z",\n'
                doc += f'  "updatedAt": null\n'
                doc += "}\n"
                doc += "```\n\n"
                doc += "**Error Responses:**\n\n"
                doc += "- Status: 404 Not Found\n"
                doc += "  - When the requested resource doesn't exist\n\n"
            elif action.lower() == "list":
                doc += f"Retrieves a list of all {entity}s.\n\n"
                doc += "**Response:**\n\n"
                doc += "- Status: 200 OK\n"
                doc += "- Content:\n\n"
                doc += "```json\n"
                doc += "[\n"
                doc += "  {\n"
                doc += f'    "id": 1,\n'
                doc += f'    "name": "string",\n'
                doc += f'    "description": "string",\n'
                doc += f'    "createdAt": "2023-01-01T00:00:00Z",\n'
                doc += f'    "updatedAt": null\n'
                doc += "  },\n"
                doc += "  {\n"
                doc += f'    "id": 2,\n'
                doc += f'    "name": "string",\n'
                doc += f'    "description": "string",\n'
                doc += f'    "createdAt": "2023-01-01T00:00:00Z",\n'
                doc += f'    "updatedAt": null\n'
                doc += "  }\n"
                doc += "]\n"
                doc += "```\n\n"
            elif action.lower() == "update":
                doc += f"Updates an existing {entity}.\n\n"
                doc += "**Parameters:**\n\n"
                doc += "- `id` (path parameter): The ID of the resource\n\n"
                doc += "**Request Body:**\n\n"
                doc += "```json\n"
                doc += "{\n"
                doc += f'  "id": 1,\n'
                doc += f'  "name": "string",\n'
                doc += f'  "description": "string"\n'
                doc += "}\n"
                doc += "```\n\n"
                doc += "**Response:**\n\n"
                doc += "- Status: 204 No Content\n\n"
                doc += "**Error Responses:**\n\n"
                doc += "- Status: 400 Bad Request\n"
                doc += "  - When the ID in the path doesn't match the ID in the request body\n"
                doc += "- Status: 404 Not Found\n"
                doc += "  - When the resource doesn't exist\n\n"
            elif action.lower() == "delete":
                doc += f"Deletes a specific {entity}.\n\n"
                doc += "**Parameters:**\n\n"
                doc += "- `id` (path parameter): The ID of the resource\n\n"
                doc += "**Response:**\n\n"
                doc += "- Status: 204 No Content\n\n"
                doc += "**Error Responses:**\n\n"
                doc += "- Status: 404 Not Found\n"
                doc += "  - When the resource doesn't exist\n\n"
            elif action.lower() == "search" or action.lower() == "filter":
                doc += f"Searches for {entity}s based on criteria.\n\n"
                doc += "**Query Parameters:**\n\n"
                doc += "- `keyword` (optional): Search keyword\n"
                doc += "- `pageNumber` (optional): Page number for pagination (default: 1)\n"
                doc += "- `pageSize` (optional): Items per page (default: 10)\n\n"
                doc += "**Response:**\n\n"
                doc += "- Status: 200 OK\n"
                doc += "- Content:\n\n"
                doc += "```json\n"
                doc += "[\n"
                doc += "  {\n"
                doc += f'    "id": 1,\n'
                doc += f'    "name": "string",\n'
                doc += f'    "description": "string",\n'
                doc += f'    "createdAt": "2023-01-01T00:00:00Z",\n'
                doc += f'    "updatedAt": null\n'
                doc += "  },\n"
                doc += "  {\n"
                doc += f'    "id": 2,\n'
                doc += f'    "name": "string",\n'
                doc += f'    "description": "string",\n'
                doc += f'    "createdAt": "2023-01-01T00:00:00Z",\n'
                doc += f'    "updatedAt": null\n'
                doc += "  }\n"
                doc += "]\n"
                doc += "```\n\n"
        
        return doc
    
    def _generate_architecture_documentation(self, db_engine: str) -> str:
        doc = "# Architecture Documentation\n\n"
        doc += "## Overview\n\n"
        doc += "This API follows a clean architecture approach with separate layers for:\n\n"
        doc += "- API Layer: Controllers, DTOs, Middleware\n"
        doc += "- Application Layer: Services, Mappings\n"
        doc += "- Domain Layer: Entities, Specifications\n"
        doc += "- Infrastructure Layer: Data Access, External Services\n\n"
        
        doc += "## Layer Responsibilities\n\n"
        
        doc += "### API Layer\n\n"
        doc += "- Handles HTTP requests and responses\n"
        doc += "- Defines API contracts (DTOs)\n"
        doc += "- Manages API-specific concerns (routing, validation, etc.)\n"
        doc += "- Implements cross-cutting concerns via middleware\n\n"
        
        doc += "### Application Layer\n\n"
        doc += "- Contains business logic\n"
        doc += "- Orchestrates domain entities to perform use cases\n"
        doc += "- Manages transaction boundaries\n"
        doc += "- Implements validation logic\n\n"
        
        doc += "### Domain Layer\n\n"
        doc += "- Defines core business entities and logic\n"
        doc += "- Contains domain-specific rules and constraints\n"
        doc += "- Independent of infrastructure concerns\n\n"
        
        doc += "### Infrastructure Layer\n\n"
        doc += "- Implements persistence mechanisms\n"
        doc += "- Handles data access and storage\n"
        doc += "- Implements external service integration\n"
        doc += "- Manages database configurations\n\n"
        
        doc += "## Database Configuration\n\n"
        doc += f"This API is optimized for {db_engine.upper()}.\n\n"
        
        if db_engine.lower() == "sqlserver":
            doc += "### SQL Server Configuration\n\n"
            doc += "The application uses SQL Server with the following optimizations:\n\n"
            doc += "- Strategic indexing for frequently queried columns\n"
            doc += "- Query splitting for complex includes\n"
            doc += "- AsNoTracking for read-only operations\n"
            doc += "- Query tagging for profiling\n"
            doc += "- Optimized paging with Skip/Take\n\n"
        elif db_engine.lower() == "oracle":
            doc += "### Oracle Configuration\n\n"
            doc += "The application uses Oracle with the following optimizations:\n\n"
            doc += "- Function-based indexes for filtering\n"
            doc += "- Optimized batch processing\n"
            doc += "- Appropriate FetchSize configuration\n"
            doc += "- Bind variables to prevent hard parsing\n"
            doc += "- Statement caching\n\n"
        
        return doc
    
    def _generate_code_documentation(self, code_examples: Dict[str, str]) -> str:
        doc = "# Code Documentation\n\n"
        
        for component, code in code_examples.items():
            doc += f"## {component.replace('_', ' ').title()}\n\n"
            doc += "```csharp\n"
            doc += code + "\n"
            doc += "```\n\n"
        
        return doc
    
    def _run(self, endpoints: List[Dict[str, Any]], db_engine: str = "sqlserver", code_examples: Dict[str, str] = None) -> str:
        documentation = {}
        
        documentation["api_documentation"] = self._generate_api_documentation(endpoints)
        documentation["architecture_documentation"] = self._generate_architecture_documentation(db_engine)
        
        if code_examples:
            documentation["code_documentation"] = self._generate_code_documentation(code_examples)
        
        return json.dumps(documentation, indent=2)

class RequirementClarificationTool(Tool):
    name = "RequirementClarifier"
    description = "Generates targeted questions to clarify requirements and identify gaps"
    
    def _generate_questions_for_user_story(self, user_story: Dict[str, Any]) -> List[str]:
        """Generate clarification questions for a specific user story"""
        
        questions = []
        
        # Basic user story elements
        if not user_story.get("title"):
            questions.append(f"What is the title for this user story?")
        
        if not user_story.get("description"):
            questions.append(f"Can you provide a user story description in the format 'As a [role], I want [feature] so that [benefit]'?")
        
        if not user_story.get("acceptance_criteria") or len(user_story.get("acceptance_criteria", [])) < 2:
            questions.append(f"What are the specific acceptance criteria for the '{user_story.get('title', 'user story')}'?")
        
        if not user_story.get("priority"):
            questions.append(f"What is the priority level (Critical, High, Medium, Low) for the '{user_story.get('title', 'user story')}'?")
        
        # Domain-specific questions based on title/description keywords
        description = user_story.get("description", "").lower()
        title = user_story.get("title", "").lower()
        
        # Data-related questions
        if any(word in description or word in title for word in ["create", "add", "new", "insert"]):
            questions.append(f"What specific data fields are required when creating a new {title.split()[-1] if title else 'item'}?")
            questions.append(f"Are there any validation rules for these fields?")
            questions.append(f"Should duplicates be prevented? If so, what fields determine uniqueness?")
        
        # Query/search questions
        if any(word in description or word in title for word in ["search", "find", "filter", "list", "view"]):
            questions.append(f"What search/filter criteria should be supported?")
            questions.append(f"What should the default sorting order be?")
            questions.append(f"Is pagination required? If so, what is the default page size?")
            questions.append(f"Are there any performance requirements for this search operation?")
        
        # Update questions
        if any(word in description or word in title for word in ["update", "edit", "modify", "change"]):
            questions.append(f"Which fields should be updatable?")
            questions.append(f"Should the system maintain a history of changes?")
            questions.append(f"Are there any fields that should be immutable after creation?")
        
        # Delete questions
        if any(word in description or word in title for word in ["delete", "remove"]):
            questions.append(f"Should deletes be logical (soft delete) or physical?")
            questions.append(f"Are there any dependencies that should prevent deletion?")
            questions.append(f"Is there a restoration process needed for deleted items?")
        
        # Security questions
        if any(word in description or word in title for word in ["security", "permission", "access", "auth"]):
            questions.append(f"What roles should have access to this functionality?")
            questions.append(f"Are there any specific permissions needed for this operation?")
        
        # Add some general questions if we don't have many specific ones
        if len(questions) < 3:
            questions.append(f"Are there any edge cases or error scenarios that should be explicitly handled?")
            questions.append(f"Are there any specific performance requirements for this feature?")
            questions.append(f"How should the system behave if an error occurs during this operation?")
        
        return questions
    
    def _generate_questions_for_technical_requirement(self, tech_req: Dict[str, Any]) -> List[str]:
        """Generate clarification questions for a technical requirement"""
        
        questions = []
        
        if not tech_req.get("title"):
            questions.append("What is the title for this technical requirement?")
            
        if not tech_req.get("description"):
            questions.append("Can you provide a detailed description of this technical requirement?")
            
        if not tech_req.get("details") or len(tech_req.get("details", [])) < 2:
            questions.append(f"What are the specific technical details for implementing '{tech_req.get('title', 'this requirement')}'?")
        
        # Domain-specific questions
        title = tech_req.get("title", "").lower()
        description = tech_req.get("description", "").lower()
        
        # Database questions
        if any(word in title or word in description for word in ["database", "data", "storage", "entity", "model"]):
            questions.append("What specific database technologies must be supported?")
            questions.append("Are there any specific indexing requirements?")
            questions.append("What is the expected data volume and growth rate?")
            questions.append("Are there any specific backup or recovery requirements?")
        
        # Performance questions
        if any(word in title or word in description for word in ["performance", "optimize", "fast", "efficient", "speed"]):
            questions.append("What are the specific performance targets (response time, throughput, etc.)?")
            questions.append("Under what load conditions should these targets be met?")
            questions.append("Are there any specific performance optimization techniques that should be used?")
        
        # Security questions
        if any(word in title or word in description for word in ["security", "secure", "auth", "protect"]):
            questions.append("What specific security standards or regulations must be complied with?")
            questions.append("Are there any specific encryption requirements?")
            questions.append("How should authentication and authorization be implemented?")
        
        # Integration questions
        if any(word in title or word in description for word in ["integration", "connect", "external", "api"]):
            questions.append("What systems need to be integrated with?")
            questions.append("What integration protocols or standards should be used?")
            questions.append("Are there any specific error handling requirements for failed integrations?")
        
        return questions
    
    def _generate_questions_for_non_functional_requirement(self, nfr: Dict[str, Any]) -> List[str]:
        """Generate clarification questions for a non-functional requirement"""
        
        questions = []
        
        if not nfr.get("title"):
            questions.append("What is the title for this non-functional requirement?")
            
        if not nfr.get("description"):
            questions.append("Can you provide a detailed description of this non-functional requirement?")
            
        if not nfr.get("details") or len(nfr.get("details", [])) < 2:
            questions.append(f"What are the specific details for '{nfr.get('title', 'this non-functional requirement')}'?")
        
        # Domain-specific questions
        title = nfr.get("title", "").lower()
        
        if "performance" in title:
            questions.append("What are the specific performance metrics that must be met?")
            questions.append("Under what conditions should these performance metrics be measured?")
            
        elif "scalability" in title:
            questions.append("What is the expected growth rate of the system?")
            questions.append("What specific scalability requirements must be met?")
            
        elif "security" in title:
            questions.append("Are there any specific security standards or certifications required?")
            questions.append("What types of security testing should be performed?")
            
        elif "availability" in title:
            questions.append("What is the required uptime percentage?")
            questions.append("Is there a specific disaster recovery time objective?")
            
        elif "usability" in title:
            questions.append("Are there any specific usability testing requirements?")
            questions.append("What accessibility standards must be met?")
            
        return questions
    
    def _generate_general_requirements_questions(self, requirements: Dict[str, Any]) -> List[str]:
        """Generate general questions about the overall requirements"""
        
        questions = []
        
        # Database choice questions
        if "database_requirements" in requirements:
            db_reqs = requirements["database_requirements"]
            if "existing_infrastructure" in db_reqs and "SQL Server" in db_reqs["existing_infrastructure"] and "Oracle" in db_reqs["existing_infrastructure"]:
                questions.append("Your environment includes both SQL Server and Oracle. Which should be the primary database for this project?")
        else:
            questions.append("What database engine should be used for this project (SQL Server, Oracle, etc.)?")
        
        # Architecture questions
        if not any(key.startswith("technical_") for key in requirements.keys()):
            questions.append("Are there any specific architectural patterns or approaches that should be followed?")
        
        # Integration questions
        if "integration_requirements" not in requirements:
            questions.append("Are there any existing systems that this API needs to integrate with?")
        
        # Security questions
        if "security" not in str(requirements).lower():
            questions.append("What authentication and authorization requirements are there for this API?")
        
        # Performance questions
        if "performance_requirements" not in requirements:
            questions.append("Are there any specific performance requirements for the API?")
        
        # Deployment questions
        if "deployment" not in str(requirements).lower():
            questions.append("What is the target deployment environment for this API?")
        
        return questions
    
    def _run(self, requirements: Dict[str, Any]) -> str:
        """Generate a comprehensive set of clarification questions based on the provided requirements"""
        
        all_questions = []
        
        # General questions about the overall requirements
        general_questions = self._generate_general_requirements_questions(requirements)
        if general_questions:
            all_questions.append({
                "section": "General Requirements",
                "questions": general_questions
            })
        
        # User story specific questions
        if "user_stories" in requirements:
            for user_story in requirements["user_stories"]:
                questions = self._generate_questions_for_user_story(user_story)
                if questions:
                    story_id = user_story.get("id", "")
                    story_title =# Specialized tools for code analysis, generation, and testing
