```toml
name = 'New Request'
method = 'GET'
url = '{{url}}'
sortWeight = 2000000
id = '60f88643-0bf5-4acd-83e0-f501862aa629'

[body.graphQL]
query = '''
query {
    launchesPast(limit: 10) {
        mission_name
        launch_date_local
        launch_site {
            site_name_long
        }
        links {
            article_link
            video_link
        }
        rocket {
            rocket_name
        }
    }
}
'''
variables = '''
{
  "find": {
    "status": "destroyed"
  }
}'''
```
