=== PanelTest ===

An example project for [https://stackoverflow.com/questions/67626789/how-does-a-stackpanel-know-when-to-re-layout](this) stackoverflow question.

The left side is a UWP Grid; the right side is a custom Panel-derived class using the Yoga library
for layout.  Click either of the two lower buttons and both buttons will have 10 added to their
height.

Without doing anything special, the Grid on the left resizes its contents automatically:

<a href="screenshots/noinvalidate.png"><img src="screenshots/noinvalidate.png" alt="InvalidateMeasure is not called" width="600"/></a>

If I manually call InvalidateMeasure after changing the size, then things work correctly:

<a href="screenshots/noinvalidate.png"><img src="screenshots/noinvalidate.png" alt="InvalidateMeasure is called" width="600"/></a>

How does the Grid invalidate its measure/layout?
