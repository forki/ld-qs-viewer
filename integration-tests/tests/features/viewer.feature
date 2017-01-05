Feature: Viewer features

Scenario: Query with a single vocabulary term
  Given I have published quality statements annotated with vocabulary term Community health care
  And I have published a quality statement annotated with vocabulary term Critical care
  When I visit the statement finder homepage
  Then I should see the total count as "Total number of NICE quality statements: 3"
  And I select the vocabulary Service Area
  And I select this single vocabulary term from the Service Area filters
  And I perform a search
  And I should see the quality statements that are annotated with that single term "2 filtered items"

Scenario: Query using multiple terms from one vocabulary
  Given I have published quality statements annotated with vocabulary term Community health care
  And I have published a quality statement annotated with vocabulary term Critical care
  When I visit the statement finder homepage
  Then I should see the total count as "Total number of NICE quality statements: 3"
  And I select the vocabulary Service Area
  And I select multiple vocabulary terms from the Service Area filters
  And I perform a search
  And I should see the quality statements that are annotated with that single term "3 filtered items"

Scenario: Query the knowledge-base with a combination of terms across different vocabularies
  Given I have published quality statements annotated with multiple vocabulary terms
  When I visit the statement finder homepage
  Then I should see the total count as "Total number of NICE quality statements: 3"
  And I select the vocabulary Service Area
  And I select this single vocabulary term from the Service Area filters
  And I select the vocabulary Setting
  And I select this single vocabulary term from the Setting filters
  And I perform a search
  And I should see the quality statements that are annotated with that single term "3 filtered items"

Scenario: Viewer homepage should display total KB Quality statement count
  Given I have published quality statements annotated with vocabulary term Community health care
  And I have published quality statements annotated with multiple vocabulary terms
  When I visit the statement finder homepage
  Then I should see the total count as "Total number of NICE quality statements: 5"

Scenario: Viewer homepage should display results ordered by Standard number then Statement number
  Given I have published some Quality Statements with different Standard and Statement numbers
  When I visit the statement finder homepage
  And I select the vocabulary Service Area
  And I select this single vocabulary term from the Service Area filters
  And I perform a search
  Then I should see the results ordered by Standard number then Statement number
