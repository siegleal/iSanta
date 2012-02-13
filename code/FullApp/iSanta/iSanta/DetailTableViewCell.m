//
//  DetailTableViewCell.m
//  iSanta
//
//  Created by Jack Hall on 2/12/12.
//  Copyright (c) 2012 __MyCompanyName__. All rights reserved.
//

#import "DetailTableViewCell.h"

@implementation DetailTableViewCell

@synthesize textField;

- (id)initWithStyle:(UITableViewCellStyle)style reuseIdentifier:(NSString *)reuseIdentifier
{
    self = [super initWithStyle:style reuseIdentifier:reuseIdentifier];
    if (self) {
    }
    return self;
}

- (id)initWithStyle:(UITableViewCellStyle)style reuseIdentifier:(NSString *)reuseIdentifier indexPath:(NSIndexPath *)indexPath
{
    self = [super initWithStyle:style reuseIdentifier:reuseIdentifier];
    if (self) 
    {
        textField = [[UITextField alloc] initWithFrame:CGRectMake(25, 10, 280, 30)];
//        textField.returnKeyType = UIReturnKeyDone;
        textField.enablesReturnKeyAutomatically = false;
        textField.backgroundColor = [UIColor clearColor ];
        [textField setClearButtonMode:UITextFieldViewModeWhileEditing];
        textField.autocorrectionType = UITextAutocorrectionTypeYes;
        textField.autocapitalizationType = UITextAutocapitalizationTypeSentences;
        textField.textAlignment = UITextAlignmentLeft;
        textField.keyboardAppearance = UIKeyboardAppearanceAlert;
        textField.tag = (indexPath.section * 100) + (indexPath.row);
        [self addSubview:textField];
    }
    return self;
}

- (void)setTextFieldPlaceholder:(NSString *)newPlaceHolder
{
    self.textField.placeholder = newPlaceHolder;
}

- (void)setTextFieldText:(NSString *)newText
{
    self.textField.text = newText;
}

- (void)setSelected:(BOOL)selected animated:(BOOL)animated
{
    [super setSelected:selected animated:animated];
    
/*    if (selected == YES)
    {
        NSLog(@"Hey I was selected");
        textField.enabled = true;
        [textField becomeFirstResponder];
    }*/

    // Configure the view for the selected state
}

@end
