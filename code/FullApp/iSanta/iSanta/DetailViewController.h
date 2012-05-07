//
//  DetailViewController.h
//  iSanta
//
//  Created by Jack Hall on 12/14/11.
//  Copyright (c) 2011 __MyCompanyName__. All rights reserved.
//

#import "PlacementBrain.h"
#import "ManualPlacementViewController.h"
#import "MasterViewController.h"
#import <UIKit/UIKit.h>
#import "DetailTableViewCell.h"
#import "StatsDisplayController.h"

@interface DetailViewController : UIViewController <UISplitViewControllerDelegate, UITableViewDelegate, UITableViewDataSource, UIImagePickerControllerDelegate, UIAlertViewDelegate, UINavigationControllerDelegate, UIActionSheetDelegate, UITextFieldDelegate, UIPickerViewDataSource, UIPickerViewDelegate>

@property (strong, nonatomic) id detailItem;

@property (strong, nonatomic) UIImagePickerController *pickerController;

@property (nonatomic) ManualPlacementViewController *manPlace;

@property (nonatomic, retain) IBOutlet UITableView *detailDescriptionTable;

@property (retain, nonatomic) UIImage *targetImage;
@property (nonatomic, retain) NSIndexPath *selectedIndexPath;

@property (nonatomic, retain) UIToolbar *doneToolBar;

@property (nonatomic, retain) UIPickerView *temperaturePicker;

@property (nonatomic, retain) UITextField *respondingTextField;

@property (nonatomic, retain) NSMutableArray *points;

- (IBAction)dismissKeyboard:(id)sender;

- (void)updateCoreDataModelWithString:(NSString *)text atCellIndexPath:(NSIndexPath *)indexPath;

- (NSMutableArray *)getArrayOfPointsFromDetailItem:(id)detailItem;

- (void)addPointWithXValue:(int)x andYValue:(int)y;

@end
