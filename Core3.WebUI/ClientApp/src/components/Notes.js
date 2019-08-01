import React, { Component } from 'react'
import { Container, Row, Col } from 'reactstrap';
import Note from './Note'
import NoteInput from './NoteInput'
import { getNotes, createNote } from './Fetch'
import '../styles/styles.css'

export default class Notes extends Component {
    constructor (props) {
        super(props);
        this.state = { notes: [], loading: true};
    }

    renderNotes = (notes) => {
        return (
            <div>
                {notes.map(n => (
                    <Note key={n.id} id={n.id} text={n.text}/>
                ))}
            </div>
        )
    }

    createNew = async (text) => {
        await createNote(text);
        await this.fetchNotes();
    }

    fetchNotes = async () => {
        const notes = await getNotes();
        this.setState({notes: notes, loading: false});
    }

    async componentDidMount() {
        await this.fetchNotes();
    }

    render(){
        const contents = this.state.loading
            ? <p><em>Loading...</em></p>
            : this.renderNotes(this.state.notes)

        return (      
            <Container >  
                <Row>   
                    <Col className="note-container">
                        {contents}
                    </Col>
                </Row>
                <Row>                    
                    <NoteInput create={this.createNew}/>                    
                </Row>
            </Container>
        );
    }
}