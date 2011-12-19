//
//  PickerViewController.h
//  iSANTA
//
//  Created by Jack Hall on 11/12/11.
//  Copyright (c) 2011 __MyCompanyName__. All rights reserved.
//

#import <UIKit/UIKit.h>

@interface PickerViewController : UIViewController <UIPickerViewDelegate, UIPickerViewDataSource>

@property (retain, nonatomic) IBOutlet UIPickerView *picker;
@property (retain, nonatomic) IBOutlet NSMutableArray *itemArray;
@property (retain, nonatomic) UITableView *superTable;
@property (retain, nonatomic) NSIndexPath *selectedIndexPath;
@property (retain, nonatomic) IBOutlet UIBarButtonItem *done;

-(IBAction)dismissPickerView:(id)sender;
- (id)initWithItemArray:(NSMutableArray *)inItems tableView:(UITableView *)inTable indexPath:(NSIndexPath *)inPath;
@end
