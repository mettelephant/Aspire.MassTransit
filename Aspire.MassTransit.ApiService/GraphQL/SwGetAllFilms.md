```toml
name = 'SwGetAllFilms'
method = 'GET'
url = '{{swurl}}'
sortWeight = 1000000
id = 'a951a3bd-15eb-42f3-af3b-a6f4904e0299'

[body.graphQL]
query = '''
query getAllFilms {
    allFilms {
        films {
            title
        }
    }
}'''
variables = ''
```
