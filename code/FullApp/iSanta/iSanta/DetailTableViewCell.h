//
//  DetailTableViewCell.h
//  iSanta
//
//  Created by Jack Hall on 2/12/12.
//  Copyright (c) 2012 __MyCompanyName__. All rights reserved.
//

#import <UIKit/UIKit.h>

@interface DetailTableViewCell : UITableViewCell

@property (nonatomic, retain) IBOutlet UITextField *textField;

- (id)initWithStyle:(UITableViewCellStyle)style reuseIdentifier:(NSString *)reuseIdentifier indexPath:(NSIndexPath *)indexPath;

- (void)setTextFieldPlaceholder:(NSString *)newPlaceHolder;

- (void)setTextFieldText:(NSString *)newText;

@end
