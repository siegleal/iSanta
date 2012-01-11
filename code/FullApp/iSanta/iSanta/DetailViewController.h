//
//  DetailViewController.h
//  iSanta
//
//  Created by Jack Hall on 12/14/11.
//  Copyright (c) 2011 __MyCompanyName__. All rights reserved.
//

#import <UIKit/UIKit.h>

@interface DetailViewController : UIViewController <UISplitViewControllerDelegate, UITableViewDelegate, UITableViewDataSource, UIImagePickerControllerDelegate, UIAlertViewDelegate, UINavigationControllerDelegate, UIActionSheetDelegate>

@property (strong, nonatomic) id detailItem;

@property (nonatomic, retain) IBOutlet UITableView *detailDescriptionTable;

@property (retain, nonatomic) UIImage *targetImage;
@property (nonatomic, retain) NSIndexPath *selectedIndexPath;

- (void)updateCoreDataModelWithString:(NSString *)text atCellIndexPath:(NSIndexPath *)indexPath;

@end
