# RuleSet

Oluşturulan nesnelerin birbirilerine olan bağımlılıkları ya da çakışmaları üzerine kurgulanmış bu uygulamada;

Bir liste oluşturulur
```c 
  var rs = RuleSet.NewRuleSet();
```
ve uygulanacak senaryoya göre bu listeye kendisine veya farklı bir nesneye bağımlılıkları olacak şekilde nesneler eklenir.
```c 
  rs.AddDep("a", "b");
  rs.AddDep("b", "c");
```
Ayrıca iki nesnenin aynı anda seçilemeyeceği bir çakışma da eklenebilir.
```c 
  rs.AddConflict("a", "c");
```
Ve son durumda nesnelerin eklendiği bu listenin tutarlı olup olmadığı kontrol edilir.
```c 
  rs.IsCoherent(); // true | false
```
Bu liste üzerinde işlemler gerçekleştirebilmek için `Option` sınıfından bir nesne üretilmesi gerekmektedir.
```c 
  var opts = Option.New(rs);
```
Oluşturulan listede yer alan bir nesneye `opts.Toggle("a")` metodu ile seçme işlemi yapıldığı anda, o nesnenin varsa bağımlı olduğu diğer nesneler de otomatik olarak seçilmekte veya o nesnenin seçimi kaldırıldığında o nesneye bağımlı olan tüm nesnelerin seçimi de kaldırılmaktadır.

Ayrıca seçilen bir nesnenin veya bu nesnenin bağımlı olduğu diğer nesnelerin çakışma yaşadığı bir nesne var ve seçili ise bu nesnenin seçimi de otomatik olarak kaldırılmaktadır.

Seçili olan nesnelerin bir listesi daha önce `Option` sınıfından oluşturulan nesne üzerinden;
- `List<string>` olarak

  ```c 
    opts.GetSelections();
  ```
- `string` olarak

  ```c 
    Console.WriteLine(opts.StringSlice());
  ```
alınabilir.

## Lisans

[MIT](https://choosealicense.com/licenses/mit/)
