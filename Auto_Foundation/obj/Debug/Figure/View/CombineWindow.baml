   M S B A M L   `   `   `  ����J  FAuto_Foundation, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullI)clr-namespace:Auto_Foundation.Figure.ViewAuto_Foundation.Figure.View  5   	   S.clr-namespace:Auto_Foundation.Figure.ViewModel Auto_Foundation.Figure.ViewModel  5	   	   I)clr-namespace:Auto_Foundation.Figure.ViewAuto_Foundation.Figure.View  5
   	   /    )Auto_Foundation.Figure.View.CombineWindow   5   	   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   ;local)clr-namespace:Auto_Foundation.Figure.View     5   	   D	viewModel.clr-namespace:Auto_Foundation.Figure.ViewModel     5	   	   :view)clr-namespace:Auto_Foundation.Figure.View     5
   	   _ [Microsoft.Xaml.Behaviors, Version=1.1.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a3i+http://schemas.microsoft.com/xaml/behaviors  5   	   -   5          �Name$  
MainWindow��5   $    � Title$ 	입력창��5   	   $	��700��6   $	��832��6"    � 
ResizeMode$ NoResize=�5   	    � WindowStartupLocation$ CenterScreen=�5   	    � ShowInTaskbar . $��11(�5   	   $��Tohoma)�5   	    � WindowStyle$ SingleBorderWindow=�5   	    � AllowsTransparency .   � 	Resources 5       �� MergedDictionaries	 �� DataType7   1Auto_Foundation.Figure.ViewModel.SettingViewModel-   'Auto_Foundation.Figure.View.SettingView4   .Auto_Foundation.Figure.ViewModel.LineViewModel*   $Auto_Foundation.Figure.View.LineView�� 5   
    5      �� 5      �� 5      settingViewModel��"	  6=   (��       *, +).R�5        5      �� 5      lineViewModel��"	  6:   (��       *, +).R�5        5      5!      %    5#      .��5%      � 
 � RowDefinitions
 5&   
   �� 5'      �� 5(      $��50 �6    � ColumnDefinitions 5*   
   �� 5+      $
��auto �6   �� 5,      .+�5.   
   �� $��0��6   $��1��6&   ��5/      �� *SettingViewModel+�� 51   
   $��1��6   $��0��6"   $��2��62    �� Orientation$ 
Horizontal=�52      $��Right=�6.   .+�53      �� $
��Draw��6   $��5q�6$   $��30��6/   $	��100��6;   ��54      �� *DrawBtnCommand+��55      ��  �� ElementName$ 
MainWindow���� 56      $��Cancel��6   $��5q�6&   $��30��61   $	��100��6=    �� IsCancel . ��58      �� *CancelBtnCommand+��59      �� $ 
MainWindow��5:      5<      5=      