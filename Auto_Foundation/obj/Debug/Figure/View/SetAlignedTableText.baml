   M S B A M L   `   `   `  ����J  FAuto_Foundation, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullI)clr-namespace:Auto_Foundation.Figure.ViewAuto_Foundation.Figure.View  5   	   S.clr-namespace:Auto_Foundation.Figure.ViewModel Auto_Foundation.Figure.ViewModel  5	   	   5    /Auto_Foundation.Figure.View.SetAlignedTableText   5   	   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   ;local)clr-namespace:Auto_Foundation.Figure.View     5   	   D	viewModel.clr-namespace:Auto_Foundation.Figure.ViewModel     5	   	   _ [Microsoft.Xaml.Behaviors, Version=1.1.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a3i+http://schemas.microsoft.com/xaml/behaviors  5
   	   -   5          �Name$  
SetAligned��5   	    � Title$ 문자 정렬��5   	   $	��180��6   $	��300��6$    � 
ResizeMode$ NoResize=�5   	    � WindowStartupLocation$ CenterScreen=�5   	    � ShowInTaskbar . $��11(�5   	   $��Tohoma)�5   	    � WindowStyle$ SingleBorderWindow=�5   	    � AllowsTransparency .   � 	Resources 5       �� MergedDictionaries   Key	 �� DataTypeC   =Auto_Foundation.Figure.ViewModel.SetAlignedTableTextViewModel�� 5   
    5      �� 5      �� 5      	isVisible  63   5      �� 5      �� 5       setAlignedTableTextViewModel��"	  6I   (��       *, +)5      %    5      .��5!      � 
 � RowDefinitions
 5"   
   �� 5#      $��90 �6   �� 5$      $��40 �6   .+�5&   
   �� $��정렬 옵션��6   $��5q�6"   .��5'      � 
 5(      �� 5)      $��30 �6$   �� 5*      $��30 �6$   .+�5-      � $��0��6   $��0��6$    � ColumnDefinitions 5.      �� 5/      �� 50      �� 51      .+�53      �� $��0��6!   $��0��6.   $��Center=�6>   $��Center=�6Y    �� Orientation$ 
Horizontal=�6v   .+�54      	� $��왼쪽 정렬��6a    d� 	IsChecked 6&   �� *&$SetAlignedTableTextModel.LeftAligned+��55   &   �� *LeftAlignedCheckCommand+��56   &   ��  �� ElementName$ 
SetAligned��58      �� 59      $��0��6!   $��1��6.   $��Center=�6>   $��Center=�6Y   $ 
Horizontal=�6v   .+�5:      	� $��가운데 정렬��5;   &    5:   &   �� *(&SetAlignedTableTextModel.CenterAligned+��5<   &   �� *CenterAlignedCheckCommand+��5=   %   �� $ 
SetAligned��5>      �� 5?      $��0��6!   $��2��6.   $��Center=�6>   $��Center=�6Y   $ 
Horizontal=�6v   .+�5@      	� $��오른쪽 정렬��5A   &    5@   &   �� *'%SetAlignedTableTextModel.RightAligned+��5B   &   �� *RightAlignedCheckCommand+��5C   &   �� $ 
SetAligned��5D      5E      � 5F      $��1��6   $��0��6$    5G      �� 5H      $��2* �6+   �� 5I      $��1* �6+   .+�5K      �� $��Center=�6    $��0��6;   $��0��6H   $��16,0,0,0q�6X   $+��%정렬 점 ~ 표 선 사이 거리 : ��5L       ��5N      �� 5O       �� 
TargetType" ��6$   .�5P   "   ��  �� Property � �� �� Value
 �   �6?    �� Triggers 5Q   "   �� 5R   &    �� Binding 62   �� *(&SetAlignedTableTextModel.CenterAligned+ �� Value True6m   .a�5S   *   ��  � ��
 �����6G   5T   '   5V      5X      �� 5Y      -   5        $��0��5Y      $��1��6+   $��Center=�6;    �� 
IsReadOnly 5Z      �� *(&SetAlignedTableTextModel.CenterAligned+��5[      �� * SetAlignedTableTextModel.Space+ �� Mode$ TwoWay=� �� UpdateSourceTrigger$ PropertyChanged=�.��5^      ��5_      �� 5`      " ��6$   .�5a   "   ��  � ��
 ���� 6?    5b   "   �� 5c   &    62   �� *(&SetAlignedTableTextModel.CenterAligned+ True6m   .a�5d   *   ��  � ��
 �����6G   5e   '   5g      .��5h   )   5i      5j      5k      5l      �� 5m   
   $��1��6   $��0��6"   $ 
Horizontal=�62   $��Center=�6K   $��30��6h   .+�5n      �� $��확인��6   $��40��6"   ��5o      �� *#!SetAlignedTableTextConfirmCommand+��5p      �� $ 
SetAligned���� 5q      $��종료��6   $��40��6"   $��10,0,0,0q�6-   ��5r      �� *" SetAlignedTableTextCancelCommand+��5s      �� $ 
SetAligned��5u      5v      5w      