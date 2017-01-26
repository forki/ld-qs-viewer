Feature: Ordering of Results

Scenario: Viewer homepage should display results ordered by Standard number then Statement number
Given I have published some Quality Statements with different Standard and Statement numbers
When I visit the statement finder homepage
And I select the vocabulary Service Area
And I select this single vocabulary term from the Service Area filters
And I perform a search
Then I should see the results ordered by Standard number then Statement number

@runonly
Scenario: Statements with explicit matches appear ahead of Statements with inferred matches
Given I have published some Quality Statements with explicit and inferred annotations
When I visit the statement finder homepage
And I select the vocabulary "Condition or disease"
And I select this "Acute myocardial infarction" from the filters
And I perform a search
Then I should see the results ordered by explicitly annotated terms first

Scenario: Statements with explicit matches High to low
Given I have published some Quality Statements with explicit and inferred annotations
When I visit the statement finder homepage
And I select the vocabulary "Condition or disease"
And I select this "Deep vein thrombosis" from the filters
And I select the vocabulary "Age group"
And I select this "Adults 65+ years" from the filters
And I select the vocabulary "Setting"
And I select this "Hospital" from the filters
And I perform a search
Then I should see the results ordered by the most explicit matches to the least

Scenario: Statements with Equal explicit matches order by standard number desc then statements number ascending
Given I have published some Quality Statements with explicit and inferred annotations
When I visit the statement finder homepage
And I select the vocabulary "Age group"
And I select a term that has equal number of explicit matches for different statements
Then I should see the results ordered by the standard number desc then statements number ascending

Scenario: Statements with Equal inferred matches order by standard number desc then statements number ascending
Given I have published some Quality Statements with explicit and inferred annotations
When I visit the statement finder homepage
And I select the vocabulary "Age group"
And I select a term that has equal number of explicit matches for different statements
Then I should see the results ordered by the standard number desc then statements number ascending
