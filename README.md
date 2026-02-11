# 📌 Employee Management API

API para gerenciamento de **colaboradores** e **departamentos**, permitindo cadastro, consulta e organização hierárquica de departamentos.

---

## 🚀 Tecnologias

### Backend (API)
- .NET 10
- ASP.NET Core Web API
- Entity Framework Core
- MediatR
- Swagger 
- Postgresql
- FluentResults
- FluentValidators
- Problem Details

### Frontend (Web)
- Razor Pages

## ▶️ Como executar

```bash
git clone
cd EmployeeManagement
dotnet restore
dotnet run
docker-compose up -d
dotnet ef database update
```

# 🧩 Endpoints

## 🏢 Department

### ➕ Criar departamento
**POST** `/api/Department`

Cria um novo departamento.

#### Body
```json
{
  "nome": "Financeiro",
  "managerId": "uuid",
  "parentDepartmentId": "uuid"
}
```

#### Response 201
```json
{
  "id": "uuid",
  "nome": "Financeiro",
  "managerId": "uuid",
  "parentDepartmentId": "uuid"
}
```

---

### 📄 Listar departamentos
**GET** `/api/Department`

Lista departamentos com filtros opcionais.

#### Query params
| Param | Tipo | Descrição |
|------|------|-----------|
| nome | string | Filtro por nome |
| managerName | string | Filtro por gerente |
| parentDepartmentName | string | Filtro por departamento pai |

#### Response 200
```json
{
  "departments": [
    {
      "id": "uuid",
      "nome": "Financeiro",
      "managerName": "João",
      "parentDepartmentName": "Administrativo"
    }
  ],
  "totalCount": 1
}
```

---

### 🌳 Listar subdepartamentos
**GET** `/api/Department/{id}/subdepartments`

Retorna subdepartamentos recursivamente.

---

## 👤 Employee

### ➕ Criar funcionário
**POST** `/api/Employee`

#### Body
```json
{
  "nome": "José",
  "cpf": "00000000000",
  "rg": "123456",
  "departmentId": "uuid"
}
```

---

### 📄 Listar funcionários
**GET** `/api/Employee`

#### Query params
| Param | Tipo |
|------|------|
| nome | string |
| cpf | string |
| rg | string |
| departmentId | uuid |

---

# ⚠️ Erros de validação

```json
{
  "title": "Erro de validação",
  "status": 422,
  "errors": {
    "campo": ["mensagem"]
  }
}
```


