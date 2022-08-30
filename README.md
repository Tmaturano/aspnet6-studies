# aspnet6-studies (balta.io)
- Minimal APIs, MVC, CRUD and Entity Framwork, Authentication/Authorization, Configuration, Performance and others. 

### Azure Commands:
- https://balta.io/blog/azure-github-actions

### Other Commands
Create publishing credentials on Azure
```
az ad sp create-for-rbac --name "<<CREDENTIAL_NAME>>" --role contributor --scopes /subscriptions/<<SUBSCRIPTION>>/resourceGroups/<<RESOURCE_GROUP_NAME>> --sdk-auth
```

List SQL Servers
```
az sql server list --resource-group <<RESOURCE_GROUP_NAME>> -o table
```

List allowed IPs in SQL Server
```
az sql server firewall-rule list --server <<SERVER_NAME>> --resource-group <<RESOURCE_GROUP_NAME>> -o table
```

Add an IP to SQL Server
```
az sql server firewall-rule create --resource-group <<RESOURCE_GROUP_NAME>> --server <<SERVER_NAME>> --name ghactions --s
```
