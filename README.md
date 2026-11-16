# Abyss

심연을 탐험하며 몬스터와 싸우고, 얻은 보물로 영지를 발전시키는 액션 게임입니다.

| 항목 | 내용 |
|---|---|
| 인원 | 3인 팀 프로젝트 |
| 개발 기간 | 2026.06 ~ 2026.07 |
| 플랫폼 | PC |
| 사용 기술 | Unity, C#, ScriptableObject, SerializeReference, UniTask |
| 플레이 영상 | (링크 추가 예정) |

<!-- 스크린샷: ![전투 화면](Docs/screenshot_battle.png) -->

## 담당 파트

제가 작성한 코드는 [`Assets/02_Scripts/Biotite`](Assets/02_Scripts/Biotite)에 있습니다.

| 파트 | 주요 파일 |
|---|---|
| 스킬, 효과 시스템 | `Core/Skiill/SkillBase.cs`, `SkillSystem.cs`, `Datas/Skills/SkillData.cs`, `Datas/Effect/*` |
| 스탯 시스템 | `Unit/StatBase.cs`, `ModifireStat.cs`, `ResourceStat.cs`, `UnitStatContainer.cs` |
| 무기 FSM, 패링 | `Core/Item/Equipments/Weapon Motion/*` |
| 격자 인벤토리 | `UI/Inventory/InventorySystem.cs`, `MouseObject.cs` |
| 장비 장착 | `Unit/Equipment/EquipmentSystem.cs`, `UI/EquipmentUI/*` |
| 기타 | 매니저 초기화(`Managers.cs`), 오브젝트 풀링(`System/Pooling`), 디버그 콘솔(`System/ConsoleSystem.cs`) |

## 주요 구현

### 스킬, 효과 시스템

언리얼 GAS가 발동(Ability)과 효과(Effect)를 나누는 방식을 참고해서, 스킬은 "언제 발동하는지"만 맡고 실제 효과는 데이터로 조합하게 만들었습니다.

```
SkillData (ScriptableObject)
 ├─ 마나, 쿨타임, 범위, 발동 트리거 (ATTACK / PARRYING / ONHIT)
 └─ List<EffectExcution>          ← 실행 방식
      ├─ ImpactEffect   : 즉시 실행
      └─ DurationEffect : 지속 시간 동안 틱마다 실행
           └─ List<EffectBase>    ← 실제 효과
                Damage, Heal, Buff, Bleed, Ignite, Poison, Frozen, Particle, Return
```

- 스킬은 공격, 패링 성공, 피격 이벤트에 맞춰 자동으로 발동합니다. `SkillBase`가 이벤트를 구독하고 쿨타임과 마나를 체크합니다.
- 효과 리스트는 `[SerializeReference]`와 `SubclassSelector`로 인스펙터에서 골라 조합합니다.
- 같은 효과 구조를 소모품(`ConsumableData`)에도 그대로 써서, 포션도 효과 조합으로 만듭니다.
- 프로젝트 규모를 고려해 GAS의 Tag 시스템과 네트워크 예측은 넣지 않았습니다.

### 스탯 시스템

- `ModifireStat`: 기본값에 수정자(`ADD`, `PERCENT_ADD`)를 쌓아서 계산합니다. 값이 바뀌었을 때만 다시 계산합니다(dirty 플래그).
- `ResourceStat`: HP, MP처럼 깎이고 차는 스탯입니다. 최대값 스탯의 변경 이벤트를 구독해서, 장비로 최대 HP가 줄어도 현재 HP가 넘치지 않게 맞춥니다.
- 수정자는 출처(`Resource`)를 함께 저장해서, 장비를 벗으면 그 장비가 준 수정자만 지웁니다.

### 무기별 애니메이션 데이터

무기마다 모션이 다르지만, 무기 종류별로 클래스를 따로 만들지 않고 애니메이션 정보를 데이터로 분리했습니다.

- `WeaponMotionData`(ScriptableObject)에 무기 전용 Animator Controller와 대기, 공격, 패링 애니메이션 이름을 넣습니다.
- `WeaponData`가 이 모션 데이터를 참조하고, 무기를 장착하면 손 Animator의 Controller를 해당 무기 것으로 교체합니다.
- 공격 애니메이션은 배열로 받아서 개수만큼 공격 상태를 만들기 때문에, 무기마다 콤보 수가 달라도 같은 코드로 처리합니다.
- 장착을 해제하면 기본 손 Animator로 되돌리고, 애니메이션 이벤트 구독도 해제합니다.

새 무기는 모션 데이터 에셋만 만들어 연결하면 FSM 코드를 수정하지 않고 추가할 수 있습니다.

### 무기 FSM, 패링

- `WeaponStateMachine`이 대기, 공격, 패링 상태를 관리합니다.
- 애니메이션 이벤트(`ATTACK`, `PARRYING_START`, `PARRYING_END`, `FINISH`)를 현재 상태에 전달해서 타격 판정과 상태 전환 시점을 맞춥니다.
- 패링은 `PARRYING_START`와 `PARRYING_END` 사이에서만 성공합니다.

### 격자 인벤토리

- 2차원 배열로 칸마다 아이템 참조를 저장해서, 크기가 다른 아이템의 배치와 겹침을 검사합니다.
- 우클릭으로 아이템을 회전할 수 있고, 겹친 아이템이 있으면 서로 바꿔 듭니다.
- 큰 아이템을 잡을 때 포인터가 중심에 고정돼 배치가 어긋나던 문제가 있었습니다. 잡은 칸을 기준으로 계산하도록 바꿔서 해결했습니다.

## 저장소 안내

- 팀 프로젝트라 다른 팀원의 코드도 함께 있습니다. 제 코드는 위의 `Biotite` 폴더입니다.
- 에셋 스토어 리소스가 포함되어 있습니다.
