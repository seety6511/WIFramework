# WIFramework<br>
<b>유니티- 친화적으로 유니티- 코딩을 편하게 만들어 주는 라이브러리입니다.<br></b>
<t>@기능 및 구조 구현에 있어서 필요한 준비물들을 대신 준비해주는 것이 주 목적입니다.<br>

<b>지원 되는 기능</b>
1. DI : 의존성 주입에 관련된 기능입니다. 아래는 WIManager를 통해 Inject 되는 Type의 리스트 입니다.<br>
<t>1-1. SingleBehaviour<br>
<t>1-2. UIBehaviour<br>
2. SingleBehaviour : WIManager 에서 관리되는 약한 수준의 싱글턴입니다.<br>
<t>2-1. TrashBehaviour : 만약 이미 존재하는 Type의 SingleBehaviour가 생성되었을경우, 이 클래스로 변환시킵니다.<br>
3. IKeyboardActor : 현재 유저가 입력하고 있는 KeyCode를 받는 interface. 이벤트를 필요로 하는 클래스에서 상속받으면 됩니다. <br>
<t>3-1. IGetKey : Input.GetKey<br>
<t>3-2. IGetKeyUp : Input.GetKeyUp<br>
<t>3-3. IGetKeyDown : Input.GetKeyDown<br>
4. SDictionary : 단순한 Dictionary Serializer class 입니다. 사용시 Inspector에서 내부가 보입니다.<br>

<b>지원 하고 싶은 기능</b>
1. Array DI : Type만을 대상으로 하여 다수의 Instance를 DI 해주는 기능입니다. <br>
<t> 1-1 UIBehaviour : ChildObjects 만이 대상입니다.<br>
<t> 1-2 WIBehaviour : Scene 전체가 대상입니다.<br>
2. Action Logger : 지정한 클래스를 대상으로 이벤트가 있을 때 마다 기록하여 텍스트 파일로 만드는 기능입니다. <br>
3. Labeling : Hierarchy 상의 Object 들의 name을 기준으로 그룹화 시켜주는 기능입니다. <br>

<b>테스트 중 인 기능<b/>
1. Lender : 특정한 객체를 대상으로 소유권의 개념을 적용시켜주는 기능입니다. <br>
2. MaterialExtractor : 모델링에 내장된 Material을 자동으로 Extract하고, 한 폴더에 모델과 함께 모아주는 기능입니다. <br>

<B>!Notification!</B><br>
<t>- Unity Cycle에 종속적입니다. WIManager가 Scene에 없으면 정상 작동하지 않습니다.<br> 
