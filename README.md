# WIFramework 1.0.0
## 유니티- 친화적으로 유니티- 코딩을 편하게 만들어 주는 라이브러리.
> * 기능 및 구조 구현에 있어서 필요한 준비물들을 대신 준비해주는 것이 주 목적입니다.
* * *

## HowToUse
> MonoBehaviour 의 대체 클래스인 WIBehaviour를 중심으로 구성된 라이브러리 입니다.   
> 라이브러리지만 이름이 Framework 인 이유는 주요 기능들이 WIBehaviour를 중심으로 돌아가기 때문입니다.   
> 권장하는 사용법을 꽤 엄격하게 따라야(하지만 강제되지는 않는) 정상적인 기능을 하므로 Framework로 명명했습니다.   
> 배우지 않아도 사용할 수 있는 라이브러리를 지향합니다. 
* * *

## 1.0.0
* **DI (Dependency Injection)**
> * 특정 class 를 대상으로 진행되는 의존성 주입 기능입니다.
> * 지원 되는 대상은 아래와 같습니다.
>> + **UIBehaviour, SingleBehaviour, Transform, RectTransform** 
>> * UIBehaviour : UGUI Component(Button, Image, Text, TMP...) 입니다. 변수명과 씬 상의 오브젝트 이름이 같아야 합니다.
>> * Transforms : 변수 명과 씬 상의 오브젝트 이름이 같아야 합니다. 자식오브젝트들을 대상으로 합니다.
> * DITracking
>> + 초기에 못한 의존성 주입 대상이 Instancing 되었을 때 주입을 시켜줍니다.
* **LowSingleton (SingleBehaviour)**
> * 라이브러리 내 에서는 SingleBehaviour class 입니다.
> * 클래스 별로 생성에 제한을 두지 않고 WIManager 내부에서 단일 Instance를 보장해 줍니다.
> * 만일 중복되는 객체가 생성되었을 경우 해당 객체를 TrashBehaviour 로 변환 시키고 이전의 정보와 Original을 트래킹 합니다.
* **KeyboardHooker**
> 키 입력을 받아 특정 Interface 를 상속받은 객체들에게 넘겨주는 기능입니다.
> 대상이 되는 Interface들은 아래와 같습니다.
>> + **IGetKey, IGetKeyUp, IGetKeyDown**
* **SDictionary**
> * Dictionary Serialize 를 위한 래핑 클래스 입니다.
* * *

## 지원 하고 싶은 기능
> 1. Array DI : 특정 Type의 class들을 대상으로 하여 다수의 Instance를 DI 해주는 기능입니다. 
> 2. Action Logger : 지정한 클래스를 대상으로 이벤트가 있을 때 마다 기록하여 텍스트 파일로 만드는 기능입니다. 
> 3. Labeling : Hierarchy 상의 Object 들의 name을 기준으로 그룹화 시켜주는 기능입니다. 
> 4. Lender : 특정한 객체를 대상으로 소유권의 개념을 적용시켜주는 기능입니다.
> 6. MaterialExtractor : 모델링에 내장된 Material을 자동으로 Extract하고, 한 폴더에 모델과 함께 모아주는 기능입니다. 

