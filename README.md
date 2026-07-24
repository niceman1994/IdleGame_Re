# 주요 구현 내용
## FSM(State Machine)
<details>
<summary>자세히 보기</summary>

- Boss의 일반 공격과 특수 공격을 조건문으로 관리하던 중 특수 공격 전환 과정에서 문제가 발생했습니다.
- 상태 판별과 상태별 동작을 분리하기 위해 FSM을 적용했습니다.
- 일반 공격, 특수 공격(Cast), 사망 상태를 각각 독립적으로 관리하도록 변경했습니다.
</details>

## Object Pool
- 몬스터와 Boss를 오브젝트 풀로 관리하여 Instantiate/Destroy 호출을 최소화했습니다.

## Buff / Skill 분리
- Buff와 Skill을 분리하여 새로운 버프와 스킬을 쉽게 추가할 수 있도록 설계했습니다.

## Inventory
<details>
<summary>자세히 보기</summary>

- 드래그 앤 드롭으로 아이템을 이동할 수 있도록 구현했습니다.
- 같은 아이템은 최대 개수까지 자동으로 합쳐지도록 구현했습니다.
- 우클릭을 통해 아이템 교환 및 사용 기능을 구현했습니다.
</details>

## Player / Monster 공통 Object 클래스
- Player와 Monster의 공통 기능을 Object 추상 클래스로 분리하여 중복 코드를 줄였습니다.

## ScriptableObject 기반 데이터 관리
- 플레이어, 몬스터, 아이템, 스킬 데이터를 ScriptableObject로 관리했습니다.
