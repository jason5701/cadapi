   M S B A M L   `   `   `  ����J  FAuto_Foundation, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullI)clr-namespace:Auto_Foundation.Figure.ViewAuto_Foundation.Figure.View  5      -    'Auto_Foundation.Figure.View.SettingView   5      R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      ;local)clr-namespace:Auto_Foundation.Figure.View     5      _ [Microsoft.Xaml.Behaviors, Version=1.1.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a3i+http://schemas.microsoft.com/xaml/behaviors  5	      -   5          >�Name$  	SettingUC��5
      
�������*  $Microsoft.Xaml.Behaviors.Interaction   Triggers 5      +  %Microsoft.Xaml.Behaviors.EventTrigger  5   
      	EventName$ Loaded��6   *  $Microsoft.Xaml.Behaviors.TriggerBase   Actions. 5      2  ,Microsoft.Xaml.Behaviors.InvokeCommandAction     Command 6$   �� *LoadSettingViewCommand+   CommandParameter 5   $   ��  �� ElementName$ 
MainWindow��5      5      .��5      �  � RowDefinitions 5   
   �� 5      $
��auto �6   �� 5      $
��auto �6    � ColumnDefinitions 5   
   �� 5      $
��auto �6   �� 5      .+�5   
   �� $��0��6   $��0��6    $��2��60   $��FDN Size(mm)��6A   $��5q�6W   .��5      �  5      �� 5      $
��auto �6'   �� 5      $	��100 �6'   �� 5       $
��auto �6'    5"      �� 5#      $��40 �6$   �� 5$      $��40 �6$   �� 5%      $��40 �6$   �� 5&      $��40 �6$   �� 5'      $��40 �6$   �� 5(      $��40 �6$   �� 5)      $��40 �6$   �� 5*      $��40 �6$   �� 5+      $��40 �6$   .+�5.      �� $��0��6   $��0��6)   $��Bottom FDN Length:��69   $��Center=�6S   �� 5/      $��0��6   $��1��6'   $��Center=�52      	 �� TextAlignment$	 Right=�65   $��5,0,0,0q�6K   ��50      �� *+)OctagonModel.SettingModel.BottomFDNLength+
 �� Mode$
 TwoWay=� �� UpdateSourceTrigger$ PropertyChanged=�.��52   \    53        54      $ TextChanged��6)   . 55         64   �� *%#BottomFDNLength_TextChanged_Command+ 56       �� $ 
MainWindow��57      58      .��59   .   5:      �� 5;      $��1��6   $��0��6)   $��Bottom FDN Center:��69   $��Center=�6S   �� 5<      $��1��6   $��1��6'    �� 
IsReadOnly . 
�������$��Center=�5>      $	 Right=�65   $��5,0,0,0q�6K   ��5=      �� *+)OctagonModel.SettingModel.BottomFDNCenter+$
 TwoWay=�$ PropertyChanged=�5>   K   �� 5?      $��2��6   $��0��6)   $��Bottom FDN Height:��69   $��Center=�6S   �� 5@      $��2��6   $��1��6'   $��Center=�5B      $	 Right=�65   $��5,0,0,0q�6K   ��5A      �� *+)OctagonModel.SettingModel.BottomFDNHeight+$
 TwoWay=�$ PropertyChanged=�.��5B   ]    5C        5D      $ TextChanged��6)   . 5E         64   �� *%#BottomFDNHeight_TextChanged_Command+ 5F   1   �� $ 
MainWindow��5G      5H      .��5I   .   5J      �� 5M      $��3��6   $��0��6)   $��Top FDN Length:��69   $��Center=�6P   �� 5N      $��3��6   $��1��6'   $��Center=�5P      $	 Right=�65   $��5,0,0,0q�6K   ��5O      �� *(&OctagonModel.SettingModel.TopFDNLength+$
 TwoWay=�$ PropertyChanged=�.��5P   \    5Q        5R      $ TextChanged��6)   . 5S         64   �� *" TopFDNLength_TextChanged_Command+ 5T      �� $ 
MainWindow��5U      5V      .��5W   .   5X      �� 5Y      $��4��6   $��0��6)   $��Top FDN Center:��69   $��Center=�6P   �� 5Z      $��4��6   $��1��6'    . 
�������$��Center=�5\   
   $	 Right=�6%   $��5,0,0,0q�6;   ��5[   
   �� *(&OctagonModel.SettingModel.TopFDNCenter+$
 TwoWay=�$ PropertyChanged=�5\   ;   �� 5]      $��5��6   $��0��6)   $��Top FDN Height:��69   $��Center=�6P   �� 5^      $��5��6   $��1��6'   $��Center=�5`      $	 Right=�65   $��5,0,0,0q�6K   ��5_      �� *(&OctagonModel.SettingModel.TopFDNHeight+$
 TwoWay=�$ PropertyChanged=�.��5`   \    5a        5b      $ TextChanged��6)   . 5c         64   �� *" TopFDNHeight_TextChanged_Command+ 5d   
   �� $ 
MainWindow��5e      5f      .��5g   .   5h      �� 5k      $��6��6   $��0��6)   $��Depth:��69   $��Center=�6G   �� 5l      $��6��6   $��1��6'    . 
�������$��Center=�5n      $	 Right=�65   $��5,0,0,0q�6K   ��5m      �� *!OctagonModel.SettingModel.Depth+$
 TwoWay=�$ PropertyChanged=�5n   K   �� 5q      $��8��6   $��0��6)   $��X:��69   $��Center=�6C   �� 5r      $��8��6   $��1��6'    . 
�������$��Center=�5t      $	 Right=�65   $��5,0,0,0q�6K   ��5s      �� *OctagonModel.SettingModel.X+$
 TwoWay=�$ PropertyChanged=�5t   K   5v      5w      �� 5x   
   $��1��6   $��0��6    $��Draw Linetype��60   $��	5,260,5,5q�6G   .��5y      �  5z      �� 5{      $��40 �6$   �� 5|      $��40 �6$    5~      �� 5      $
��auto �6'   �� 5�      $	��100 �6'   �� 5�      $
��auto �6'   .+�5�      �� $��0��6   $��0��6)   $��Color:��69   $��Center=�6G   �� 5�      $��0��6   $��1��6'    . 
�������$��Center=�5�      $	 Right=�65   $��5,0,0,0q�6K   ��5�      �� *&$OctagonModel.SettingModel.ColorIndex+$
 TwoWay=�$ PropertyChanged=�5�   K   �� 5�      $��0��6   $��2��6&   $	��0.5q�66   $��Center=�5�      $��40��64   $��30��6?   $��3,0,0,0q�5�      ��5�      �� *&$OctagonModel.SettingModel.ColorValue+��5�      �� *ColorBtnCommand+��5�      �� $ 
MainWindow��5�      �� 5�      $��1��6   $��0��6)   $��	Linetype:��69   $��Center=�6J   �� 5�      $��1��6   $��1��6(   $��2��5�      $��Center=�6/   $	��140��6J   $��5,0,0,0q�5�       �� SelectedItem 5�      �� *,*OctagonModel.SettingModel.SelectedLinetype+$
 TwoWay=�$ PropertyChanged=���5�      �� *%#OctagonModel.SettingModel.Linetypes+5�      5�      5�      �� 5�   
   $��0��6   $��1��6    $��2��60   $��미리보기��6A   $��5q�6O   .��5�      �  5�      �� 5�      $��1* �6$   �� 5�      $��1* �6$   .+�5�      �� -   5         ��Name$
 Plan��5�   &   $��0��6   $	��380��64   $	��280��6@   �� 5�      -   5        $ Section��5�   &   $��1��6   $	��380��67   $	��280��6C   5�      5�      5�      5�      