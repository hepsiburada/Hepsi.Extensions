# Hepsi.Extensions v0.1

We have started to share our commonly used extensions for C#.
For now, dynamic filtering and ordering is available.

## Installation
Nuget link: https://www.nuget.org/packages/Hepsi.Extensions/
```sh
PM> Install-Package Hepsi.Extensions
```

## Usages
Sorting and filtering by property name was a shameful process if the property is ambiguous at runtime.

### DynamicOrderBy

- IQueryable<T> DynamicOrderBy<T>(this IQueryable<T> source, string propertyName, string direction)
- IQueryable<T> DynamicOrderBy<T>(this IQueryable<T> source, OrderByRequest orderByRequest)

#### :(
```csharp
        public IQueryable<DummyDocument> SortDocuments(IQueryable<DummyDocument> documents, string propertyName)
        {
            switch (propertyName.ToLower())
            {
                case "createdat":
                    return documents.OrderBy(d => d.CreatedAt);
                case "count":
                    return documents.OrderBy(d => d.Count);
                case "id":
                    return documents.OrderBy(d => d.Id);
                case "name":
                    return documents.OrderBy(d => d.Name);
                case "complexype.dummyinteger":
                    return documents.OrderBy(d => d.ComplexType.DummyInteger);
                case "complexype.innercomplextype.dummyinteger":
                    return documents.OrderBy(d => d.ComplexType.InnerComplexType.DummyInnerInteger);

                //Can be hundred cases?... :(

                default:
                    //Can't find the property
                    return documents;
            }
        }
```

#### Cool Way
```csharp
        public IQueryable<DummyDocument> SortDocuments(IQueryable<DummyDocument> documents, string propertyName)
        {
            return documents.DynamicOrderBy(propertyName, "ascending");
        }
        
        //Or you can use our OrderByRequest type as an alternative:
        
        public IQueryable<DummyDocument> SortDocuments(IQueryable<DummyDocument> documents, string propertyName)
        {        
            return documents.DynamicOrderBy(new OrderByRequest(propertyName, OrderByDirection.Ascending));
        }
```

### DynamicFilterBy

- IQueryable<T> DynamicFiltering<T>(this IQueryable<T> source, string propertyName, string value)

#### :(     
```csharp
        public IQueryable<DummyDocument> FilterDocuments(IQueryable<DummyDocument> documents, string propertyName, string value)
        {
            switch (propertyName.ToLower())
            {
                case "count":
                    return documents.Where(d => d.Count == Int32.Parse(value));
                case "id":
                    return documents.Where(d => d.Id == Int32.Parse(value));
                case "name":
                    return documents.Where(d => d.Name == value);

                //Can be hundred cases?... :(

                default:
                    //Can't find the property
                    return documents;
            }
        }
```

#### Cool Way
```csharp
        public IQueryable<DummyDocument> FilterDocuments(IQueryable<DummyDocument> documents, string propertyName, string value)
        {
            return documents.DynamicFiltering(propertyName, value);
        }
```

> Note: Dynamic Filtering does not support complex types for now.

Thanks.
Hepsiburada Software Development Team